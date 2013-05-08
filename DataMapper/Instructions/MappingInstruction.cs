using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using DataMapper.Mapping;

namespace DataMapper.Instructions
{

    [Serializable()]
    public class MappingInstruction
    {

        public Boolean HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }
        public MappingInstruction Parent
        {
            get;
            set;
        }
        public PropertyMap ParentCollectionPropertyMap
        {
            get;
            set;
        }
        public PropertyInfo ParentCollectionReceivingPropertyInfo
        {
            get
            {
                return this.ChangeDirection == MappingDirection.SourceToTarget ?
                    this.ParentCollectionPropertyMap.TargetPropertyInfo : this.ParentCollectionPropertyMap.SourcePropertyInfo;
            }
        }

        public MappingInstructionList Children
        {
            get;
            set;
        }

        public Object ObjectReceivingChanges
        {
            get
            {
                return this.ChangeDirection == MappingDirection.SourceToTarget ? Target : Source;
            }
        }
        public Object ObjectDeliveringChanges
        {
            get
            {
                return this.ChangeDirection == MappingDirection.SourceToTarget ? Source : Target;
            }
        }
        public Type ObjectReceivingChangesType
        {
            get
            {
                return this.ChangeDirection == MappingDirection.SourceToTarget ? DataMap.TargetType : DataMap.SourceType;
            }
        }
        public Type ObjectDeliveringChangesType
        {
            get
            {
                return this.ChangeDirection == MappingDirection.SourceToTarget ? DataMap.SourceType : DataMap.TargetType;
            }
        }

        public void SetObjectReceivingChanges(Object obj)
        {
            switch (this.ChangeDirection)
            {
                case MappingDirection.SourceToTarget:
                    this.Target = obj;
                    break;
                case MappingDirection.TargetToSource:
                    this.Source = obj;
                    break;
            }
        }

        public MappingInstruction(MappingInstruction parent,PropertyMap parentCollectionPropertyMap, DataMap dataMap, MappingDirection changeDirection, Object source,Object target)
        {
            this.Parent = parent;
            this.ParentCollectionPropertyMap = parentCollectionPropertyMap;
            this.DataMap = dataMap;
            this.ChangeDirection = changeDirection;
            this.Source = source;
            this.Target = target;

            this.Children = new MappingInstructionList();

            this.SetCommandType();
        }
        private void SetCommandType()
        {
            if (this.ObjectReceivingChanges == null)
            {
                if (this.ObjectDeliveringChanges == null)
                {
                    this.CommandType = MappingInstructionType.None;
                }
                else
                {
                    this.CommandType = MappingInstructionType.Create;
                }
            }
            else
            {
                if (this.ObjectDeliveringChanges == null)
                {
                    this.CommandType = MappingInstructionType.Delete;
                }
                else
                {
                    this.CommandType = this.DataMap.ArePropertiesEqual(false, this.Source, this.Target)? MappingInstructionType.None: MappingInstructionType.Update;
                }
            }
        }
        
        //apply the changes that are in this datamapcommand
        public MappingInstructionResult ApplyChanges()
        {
            MappingInstructionResult result = new MappingInstructionResult();

            this.ApplyDeletes(result);
            this.ApplyCreates(result);
            this.ApplyUpdates(result);

            return result;
        }
        private void ApplyDeletes(MappingInstructionResult commandResult)
        {
            //process the kids first. for the deletes, we want to do this bottom up.
            this.Children.ForEach(a => a.ApplyDeletes(commandResult));

            //if we have a delete then apply it
            if (this.CommandType == MappingInstructionType.Delete)
            {
                //add the object to the list of things we are deleting
                //commandResult.ItemsDeleted.Add(new LegacyItem(this.ObjectReceivingChangesType, this.ObjectReceivingChanges));

                //add this to the list of items we are creating
                commandResult.Items.Add(new Item(MappingInstructionType.Delete, this.ChangeDirection, 
                    this.Source,DataMap.SourceType, this.Target, DataMap.TargetType, this.DataMap.PropertyMapList));

                //do a delete from our parent list if we have one
                if (this.HasParent)
                {
                    //get the parent list...
                    var receivingItemList = this.FindParentReceivingList();

                    //apply the removal
                    receivingItemList.Remove(this.ObjectReceivingChanges);
                }
            }
        }
        private void ApplyCreates(MappingInstructionResult commandResult)
        {
            //if we have an add, do the work. if not, we shouldnt return,
            //we need to process the kids
            if (this.CommandType == MappingInstructionType.Create)
            {
                //create the new object
                var newReceivingObject = this.ObjectReceivingChangesType.CreateInstance();

                //set the receiving object. this will be needed by recursive adds
                this.SetObjectReceivingChanges(newReceivingObject);

                //copy data from the delivering object
                this.DataMap.PropertyMapList.MapNonCollection(this.Source, this.Target, this.ChangeDirection);

                //do we have a parent? if so, we need to add this guy to the
                //parent list
                if (this.HasParent)
                {
                    //get the list for the parent
                    var receivingItemList = this.FindParentReceivingList();

                    //no list on the parent object, throw for now
                    //perhaps an option can fix this later (autocreate the list?)
                    if (receivingItemList.IsNull())
                    {
                        throw new DataMapperException("Unable to add item because the list on the created/existing receiving object is null. Objects with lists should create lists on instantiation.");
                    }

                    //add this newly minted object to the parents list
                    receivingItemList.Add(newReceivingObject);
                }

                //new items being tracked 
                //commandResult.ItemsAdded.Add(new LegacyItem(this.ObjectReceivingChangesType, newReceivingObject));

                //add this to the list of items we are creating
                commandResult.Items.Add(new Item(MappingInstructionType.Create, this.ChangeDirection, this.Source, this.DataMap.SourceType,
                    this.Target, this.DataMap.TargetType, this.DataMap.PropertyMapList));
            }

            //now the kids. adds have to happen 'top-down'
            this.Children.ForEach(a => a.ApplyCreates(commandResult));
        }
        private void ApplyUpdates(MappingInstructionResult commandResult)
        {
            //if we have an add, do the work. if not, we shouldnt return,
            //we need to process the kids
            if (this.CommandType == MappingInstructionType.Update)
            {
                //copy data from the delivering object
                this.DataMap.PropertyMapList.MapNonCollection(this.Source, this.Target, this.ChangeDirection);

                //add this to the list of items we are creating
                commandResult.Items.Add(new Item(MappingInstructionType.Update, this.ChangeDirection, this.Source,this.DataMap.SourceType,
                    this.Target,this.DataMap.TargetType, this.DataMap.PropertyMapList));
            }

            //updates being done top down. could be done either way
            this.Children.ForEach(a => a.ApplyUpdates(commandResult));
        }
        private IDataMapperList FindParentReceivingList()
        {
            if (this.HasParent == false)
                throw new DataMapperException("Cannot find parent receiving list because the current command does not have a parent.");

            return this.ParentCollectionReceivingPropertyInfo.ExtractIDataMapperList(this.Parent.ObjectReceivingChanges);

            //Object item=null;

            //item = this.ParentCollectionReceivingPropertyInfo.GetValue(this.Parent.ObjectReceivingChanges,null);

            //return new DataMapperList(item);
        }

        public MappingDirection ChangeDirection
        {
            get;
            set;
        }
        public MappingInstructionType CommandType
        {
            get;
            private set;
        }
        public Object Source
        {
            get;
            set;
        }
        public Object Target
        {
            get;
            set;
        }
        public DataMap DataMap
        {
            get;
            set;
        }

    }
    
    


    
    

}
