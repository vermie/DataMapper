using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using DataMapper.Mapping;

namespace DataMapper.Commands
{

    [Serializable()]
    public class Command
    {

        public Boolean HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }
        public Command Parent
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
                return this.ChangeDirection == CommandChangeDirection.ApplyChangesFromSourceToTarget ?
                    this.ParentCollectionPropertyMap.TargetPropertyInfo : this.ParentCollectionPropertyMap.SourcePropertyInfo;
            }
        }

        public CommandList ChildCommands
        {
            get;
            set;
        }

        public Object ObjectReceivingChanges
        {
            get
            {
                return this.ChangeDirection == CommandChangeDirection.ApplyChangesFromSourceToTarget ? Target : Source;
            }
        }
        public Object ObjectDeliveringChanges
        {
            get
            {
                return this.ChangeDirection == CommandChangeDirection.ApplyChangesFromSourceToTarget ? Source : Target;
            }
        }
        public Type ObjectReceivingChangesType
        {
            get
            {
                return this.ChangeDirection == CommandChangeDirection.ApplyChangesFromSourceToTarget ? DataMap.TargetType : DataMap.SourceType;
            }
        }
        public Type ObjectDeliveringChangesType
        {
            get
            {
                return this.ChangeDirection == CommandChangeDirection.ApplyChangesFromSourceToTarget ? DataMap.SourceType : DataMap.TargetType;
            }
        }

        public void SetObjectReceivingChanges(Object obj)
        {
            switch (this.ChangeDirection)
            {
                case CommandChangeDirection.ApplyChangesFromSourceToTarget:
                    this.Target = obj;
                    break;
                case CommandChangeDirection.ApplyChangesFromTargetToSource:
                    this.Source = obj;
                    break;
            }
        }

        public Command(Command parent,PropertyMap parentCollectionPropertyMap, DataMap dataMap, CommandChangeDirection changeDirection, Object source,Object target)
        {
            this.Parent = parent;
            this.ParentCollectionPropertyMap = parentCollectionPropertyMap;
            this.DataMap = dataMap;
            this.ChangeDirection = changeDirection;
            this.Source = source;
            this.Target = target;

            this.ChildCommands = new CommandList();

            this.SetCommandType();
        }
        private void SetCommandType()
        {
            if (this.ObjectReceivingChanges == null)
            {
                if (this.ObjectDeliveringChanges == null)
                {
                    this.CommandType = CommandType.None;
                }
                else
                {
                    this.CommandType = CommandType.Create;
                }
            }
            else
            {
                if (this.ObjectDeliveringChanges == null)
                {
                    this.CommandType = CommandType.Delete;
                }
                else
                {
                    this.CommandType = this.DataMap.ArePropertiesEqual(false, this.Source, this.Target)? CommandType.None: CommandType.Update;
                }
            }
        }
        
        //apply the changes that are in this datamapcommand
        public CommandResult ApplyChanges()
        {
            CommandResult result = new CommandResult();

            this.ApplyDeletes(result);
            this.ApplyCreates(result);
            this.ApplyUpdates(result);

            return result;
        }
        private void ApplyDeletes(CommandResult commandResult)
        {
            //process the kids first. for the deletes, we want to do this bottom up.
            this.ChildCommands.ForEach(a => a.ApplyDeletes(commandResult));

            //if we have a delete then apply it
            if (this.CommandType == CommandType.Delete)
            {
                //add the object to the list of things we are deleting
                commandResult.ItemsDeleted.Add(new CommandResultItem(this.ObjectReceivingChangesType, this.ObjectReceivingChanges));

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
        private void ApplyCreates(CommandResult commandResult)
        {
            //if we have an add, do the work. if not, we shouldnt return,
            //we need to process the kids
            if (this.CommandType == CommandType.Create)
            {
                //create the new object
                var newReceivingObject = this.ObjectReceivingChangesType.CreateInstance();

                //set the receiving object. this will be needed by recursive adds
                this.SetObjectReceivingChanges(newReceivingObject);

                //copy data from the delivering object
                this.Map(this.Source, this.Target, this.ChangeDirection);

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
                commandResult.ItemsAdded.Add(new CommandResultItem(this.ObjectReceivingChangesType, newReceivingObject));

                //add this to the list of items we are creating
                commandResult.SourceTargetPairList.Add(new CommandResultSourceTargetPair(this.Source, this.Target, this.DataMap.PropertyMapList));
            }

            //now the kids. adds have to happen 'top-down'
            this.ChildCommands.ForEach(a => a.ApplyCreates(commandResult));
        }
        private void ApplyUpdates(CommandResult commandResult)
        {
            //if we have an add, do the work. if not, we shouldnt return,
            //we need to process the kids
            if (this.CommandType == CommandType.Update)
            {
                //copy data from the delivering object
                this.Map(this.Source, this.Target, this.ChangeDirection);

                //add this to the list of items we are creating
                commandResult.SourceTargetPairList.Add(new CommandResultSourceTargetPair(this.Source, this.Target, this.DataMap.PropertyMapList));
            }

            //updates being done top down. could be done either way
            this.ChildCommands.ForEach(a => a.ApplyUpdates(commandResult));
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

        public CommandChangeDirection ChangeDirection
        {
            get;
            set;
        }
        public CommandType CommandType
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

        private void Map(Object source, Object target)
        {
            this.Map(source, target, this.ChangeDirection);
        }
        private void Map(Object source, Object target, CommandChangeDirection changeDirection)
        {
            foreach (var item in this.DataMap.PropertyMapList.Where(a => a.IsCollection == false))
            {
                switch (changeDirection)
                {
                    case CommandChangeDirection.ApplyChangesFromSourceToTarget:
                        item.CopySourceToTarget(source, target);
                        break;
                    case CommandChangeDirection.ApplyChangesFromTargetToSource:
                        item.CopyTargetToSource(source, target);
                        break;
                }
            }
        }
    }
    
    


    
    

}
