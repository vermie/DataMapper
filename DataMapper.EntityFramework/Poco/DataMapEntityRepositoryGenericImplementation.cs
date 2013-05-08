using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DataMapper.Instructions;
using DataMapper.Mapping;
using DataMapper;

namespace DataMapper.EntityFramework.Poco
{
    public class DataMapEntityRepositoryGenericImplementation
    {
        #region Private/Internal
        private Type _dbContextType;
        private DataMap _entityToEntityTargetDataMap;

        internal Type DbContextType
        {
            get
            {
                return this._dbContextType;
            }
            set
            {
                this._dbContextType = value;
            }
        }
        internal DataMap EntityToEntityTargetDataMap
        {
            get
            {
                return this._entityToEntityTargetDataMap;
            }
            set
            {
                this._entityToEntityTargetDataMap = value;
            }
        }

        private Object[] GetKeyValues(Object entityTarget)
        {
            return this._entityToEntityTargetDataMap.PropertyMapList.GetTargetKeyValues(entityTarget);
        }

        private DbSet EntityDbSet(DbContext context)
        {
            return context.Set(this._entityToEntityTargetDataMap.SourceType);
        }

        private DbContext CreateDbContextInstance(Boolean lazyLoadingEnabled)
        {
            var context = (DbContext)_dbContextType.Assembly.CreateInstance(_dbContextType.FullName, false);

            context.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;

            return context;
        }

        private Object TryFindEntityByEntityTarget(Object entityTarget)
        {
            return this.TryFindEntity(this.GetKeyValues(entityTarget));
        }
        private Object TryFindEntityByEntityTarget(DbContext context, Object entityTarget)
        {
            return this.TryFindEntity(context, this.GetKeyValues(entityTarget));
        }

        private Object TryFindEntity(params Object[] keyValues)
        {
            using (var context = this.CreateDbContextInstance(true))
            {
                return this.TryFindEntity(context, keyValues);
            }
        }
        private Object TryFindEntity(DbContext context, params Object[] keyValues)
        {
            //get a handle
            var entitySet = this.EntityDbSet(context);

            //find from data objects...
            var entityObject = entitySet.Find(keyValues);

            //NOTE: potentially do something with includes?
            //for now, use this as a variable to track whether we had the includes or not.
            Boolean entityObjectLoadedWithIncludes = false;

            //if we get something back but we did not have includes for the kids,
            //we need to try to manually load the sucker.
            if ((entityObject != null) && (entityObjectLoadedWithIncludes == false))
            {
                //store the value of the default lazy loading behavior in the context.
                var contextDefaultLazyLoadingValue = context.Configuration.LazyLoadingEnabled;

                //we could deal with this by either:
                //1.- enabling lazyloading in a try finally block so we can get our job done. I am not sure I like this...
                //because it is tampering with the context which is not something I like to do. However, I also know this
                //should be running in a threadsafe way so as long as I can 'undo' it, it should be ok.
                //2.- just throwing an exception

                // a throw to deal with this
                //throw new NotImplementedException("Unable to load the entity automatically.");
                try
                {
                    context.Configuration.LazyLoadingEnabled = true;

                    //walk the children to make sure they laziless load they asses.
                    this.LoadEntity(context, this._entityToEntityTargetDataMap, entityObject);
                }
                finally
                {
                    //this is disposal, not 

                    //make sure we undo this no matter what. No molesty!
                    context.Configuration.LazyLoadingEnabled = contextDefaultLazyLoadingValue;
                }

            }

            //return the entity object
            return entityObject;
        }

        private void LoadEntity(DbContext context, DataMap dataMap, Object entity)
        {
            foreach (var item in dataMap.DataMapCollectionList)
            {
                var collectionItemList = item.PropertyMap.SourcePropertyInfo.ExtractIDataMapperList(entity);
                //var collectionItemList = new DataMapperList(item.PropertyMap.SourcePropertyInfo.GetValue(entity, null));

                foreach (var collectionItem in collectionItemList)
                {
                    this.LoadEntity(context, item.ItemDataMap, collectionItem);
                }
            }
        }

        #endregion

        #region Protected

        protected void AddEntityTarget(Object entityTarget)
        {
            //- new up a context
            using (var context = this.CreateDbContextInstance(false))
            {
                var result = this.AddEntityTarget(context, entityTarget);

                //- save the changes. we do this because it represents 'one' transaction
                context.SaveChanges();

                //- remap any other changes back to the business object? or simply create a new business object and return that?
                result.Items.Copy(MappingDirection.SourceToTarget);

                //- save the memento?
                //this.TrySaveMemento(order);
            }
        }
        protected MappingInstructionResult AddEntityTarget(DbContext context, Object entityTarget)
        {
            //- do the validation?
            //TODO for this...
            //things to consider...how modular should the validation be? Validation should be divided into several types maybe?
            //1.- Property validation, needed for proper persistance. This must also be run and always be true. Handled via data annotations?
            //2.- Context based 'Always true' type rules. This might include a rule like 'An order in 'Closed' status should never be updated.
            //3.- Context based validation. Depending on the user action, things that should or should not be allowed maybe? Stuff like
            //updating an order and not including shipping should only be allowed when done as part of a larger transaction (like adding an account)
            //This should represent rules 'Outside' of the scope of the provider.


            //- create the new data objects.
            //var orderEntity = context.OrderEntity.CreateAndAdd();
            var orderEntity = this.EntityDbSet(context).CreateAndAdd(this._entityToEntityTargetDataMap.SourceType);

            //build the command
            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(
                this._entityToEntityTargetDataMap,
                MappingDirection.TargetToSource, orderEntity, entityTarget);

            //- Apply the changes from the business object to the data object.
            var result = dataMapCommand.ApplyChanges();

            return result;

            
        }

        protected void UpdateEntityTarget(Object entityTarget)
        {
            using (var context = this.CreateDbContextInstance(false))
            {
                var result = this.UpdateEntityTarget(context, entityTarget);

                //- apply the saves
                context.SaveChanges();

                //we still need to read the keys back out from the context item.
                result.Items.Copy(MappingDirection.SourceToTarget);
            }
        }
        protected MappingInstructionResult UpdateEntityTarget(DbContext context, Object entityTarget)
        {
            //load a fresh object
            var entity = this.TryFindEntityByEntityTarget(context, entityTarget);

            if (entity == null)
            {
                //cannot update it if its not there.
                throw new Exception("Cant do that their update");
            }

            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(this._entityToEntityTargetDataMap,
                MappingDirection.TargetToSource, entity, entityTarget);

            var result = dataMapCommand.ApplyChanges();

            result.ItemsDeleted.ForEach(a => context.Set(a.ObjectReceivingChangesType).Remove(a.ObjectReceivingChanges));

            return result;
        }

        protected Object TryFindEntityTarget(params Object[] keyValues)
        {
            using (var context = this.CreateDbContextInstance(false))
            {
                return TryFindEntityTarget(context, keyValues);
            }
        }
        protected Object TryFindEntityTarget(DbContext context, params Object[] keyValues)
        {
            //find the entity object
            var entity = this.TryFindEntity(context, keyValues);

            //exit out early
            if (entity == null)
                return null;

            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(
                this._entityToEntityTargetDataMap,
                MappingDirection.SourceToTarget, entity, null); // newBusinessObject);

            //get the result
            var result = dataMapCommand.ApplyChanges();

            //return the result
            return dataMapCommand.ObjectReceivingChanges;
        }

        protected Boolean EntityExists(params Object[] keyValues)
        {
            using (var context = this.CreateDbContextInstance(false))
            {
                return EntityExists(context, keyValues);
            }
        }
        protected Boolean EntityExists(DbContext context, params Object[] keyValues)
        {
            //get a handle
            var entitySet = this.EntityDbSet(context);

            //find from data objects...
            var entityObject = entitySet.Find(keyValues);

            //return the entity object
            return entityObject != null;
        }

        protected void DeleteEntityTarget(Object entityTarget)
        {
            //2.- new up a context
            using (var context = this.CreateDbContextInstance(false))
            {
                var result = this.DeleteEntityTarget(context, entityTarget);

                //-apply changes
                context.SaveChanges();
            }
        }
        protected MappingInstructionResult DeleteEntityTarget(DbContext context, Object entityTarget)
        {
            //find from data objects...
            var entity = this.TryFindEntityByEntityTarget(context, entityTarget);

            if (entity == null)
            {
                //exit out early? Or blow up?
                //blow up for now
                throw new Exception("Unable to delete item because it does not exist.");
                //return;
            }

            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(
                this._entityToEntityTargetDataMap,
                MappingDirection.TargetToSource, entity, null);

            var result = dataMapCommand.ApplyChanges();

            result.ItemsDeleted.ForEach(a => context.Set(a.ObjectReceivingChangesType).Remove(a.ObjectReceivingChanges));

            return result;
        }

        //protected void DeleteEntityTargetAll()
        //{
        //    using (var context = this.CreateDbContextInstance(false))
        //    {
        //        this.DeleteEntityTargetAll(context);
        //    }
        //}
        //protected void DeleteEntityTargetAll(DbContext context)
        //{
        //    var entitySet = this.EntityDbSet(context);
        //    List<Object> removeList = new List<object>();

        //    foreach (var item in entitySet)
        //    {
        //        removeList.Add(item);
        //    }

        //    removeList.ForEach(a => entitySet.Remove(a));

        //    context.SaveChanges();
        //}

        #endregion

        #region Public

        public DataMapEntityRepositoryGenericImplementation()
        {

        }
        public DataMapEntityRepositoryGenericImplementation(Type dbContextType, DataMap entityToEntityTargetDataMap)
        {
            if (!dbContextType.IsOrIsSubclassOf<DbContext>())
            {
                throw new Exception("Invalid context type provided");
            }

            this._dbContextType = dbContextType;
            this._entityToEntityTargetDataMap = entityToEntityTargetDataMap;
        }

        #endregion
    }
}
