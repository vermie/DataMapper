using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Instructions;
using DataMapper.Mapping;

namespace DataMapper.Repositories
{

    /// <summary>
    /// Provides an implementation for a repository that encapsulates the mapping of changes from a
    /// business objects aggregate to an entity framework aggregate. 
    /// </summary>
    /// <typeparam name="TAggregate">A type that represents the aggregate root - usually of a business or service layer.</typeparam>
    /// <typeparam name="TEntity">A type that represents the corresponding entity framework root object.</typeparam>
    /// <typeparam name="TDbContext">The database context. Note that the 'TEntity' type must be defined in this context.</typeparam>
    public abstract class EntityFrameworkRepository<TAggregate, TEntity, TDbContext> : Repository<TAggregate, TEntity>
        where TDbContext : DbContext, new()
        where TAggregate : new()
        where TEntity : new()
    {

        #region Non-Public
        private Type DbContextType
        {
            get
            {
                return typeof(TDbContext);
            }
        }

        private Object[] GetKeyValues(TAggregate aggregate)
        {
            return this.DataMap.PropertyMapList.GetSourceKeyValues(aggregate);
        }

        private DbSet EntityDbSet(DbContext context)
        {
            return context.Set(this.DataMap.TargetType);
        }

        private DbContext GetDbContextInstance()
        {
            return this.Context.DbContext;

            //var context = (DbContext)DbContextType.Assembly.CreateInstance(DbContextType.FullName, false);

            //context.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;

            //return context;
        }

        private TEntity TryFindEntityByAggregate(DbContext context, TAggregate aggregate)
        {
            return this.TryFindEntity(context, this.GetKeyValues(aggregate));
        }

        private TEntity TryFindEntity(DbContext context, params Object[] keyValues)
        {
            //NOTE: potentially do something with includes?
            //for now, use this as a variable to decide whether or not we 'automagically' load child entities.
            //later we can make this a convention driven maybe...
            Boolean autoLoadByWalkingProperties = true;

            //store the value of the default lazy loading behavior in the context.
            var contextDefaultLazyLoadingValue = context.Configuration.LazyLoadingEnabled;
            var contextProxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            Object entityObject = null;

            try
            {
                if (autoLoadByWalkingProperties)
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = true;
                }

                //get a handle
                var entitySet = this.EntityDbSet(context);

                //find from data objects...
                entityObject = entitySet.Find(keyValues);

                //if we get something back but we did not have includes for the kids,
                //we need to try to manually load the sucker.
                if ((entityObject != null) && (autoLoadByWalkingProperties == true))
                {
                    //we could deal with this by either:
                    //1.- enabling lazyloading in a try finally block so we can get our job done. I am not sure I like this...
                    //because it is tampering with the context which is not something I like to do. However, I also know this
                    //should be running in a single thread way so as long as I can 'undo' it, it should be ok.
                    //2.- just throwing an exception

                    // a throw to deal with this
                    //throw new NotImplementedException("Unable to load the entity automatically.");
                    //walk the children to make sure they laziless load they asses.
                    this.LoadChildEntities(context, this.DataMap, entityObject);
                }
            }
            finally
            {
                //make sure we undo this no matter what. No molesty!
                context.Configuration.LazyLoadingEnabled = contextDefaultLazyLoadingValue;
                context.Configuration.ProxyCreationEnabled = contextProxyCreationEnabled;
            }

            //return the entity object
            return (TEntity)entityObject;
        }

        private void LoadChildEntities(DbContext context, DataMap dataMap, Object entity)
        {
            foreach (var item in dataMap.DataMapCollectionList)
            {
                var collectionItemList = item.PropertyMap.TargetPropertyInfo.ExtractIDataMapperList(entity);
                //var collectionItemList = new DataMapperList(item.PropertyMap.SourcePropertyInfo.GetValue(entity, null));

                foreach (var collectionItem in collectionItemList)
                {
                    this.LoadChildEntities(context, item.ItemDataMap, collectionItem);
                }
            }
        }

        private List<Action> _actionJackson = new List<Action>();

        private void ChangesCommitted(object sender, EventArgs e)
        {
            //do it baby!probably fucking up some stuff about how closures work....eh
            this._actionJackson.ForEach(a => a());

            this._actionJackson.Clear();
        }

        protected void AddAggregate(TAggregate aggregate)
        {
            //- new up a context
            var context = this.GetDbContextInstance();

            var result = this.AddAggregate(context, aggregate);

            //- save the changes. we do this because it represents 'one' transaction
            //context.SaveChanges();

            //- remap any other changes back to the business object? or simply create a new business object and return that?
            //result.Items.Copy(MappingDirection.TargetToSource);

            this._actionJackson.Add(() => result.Items.Copy(MappingDirection.TargetToSource));
        }
        private MappingInstructionResult AddAggregate(DbContext context, TAggregate aggregate)
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
            var entity = this.EntityDbSet(context).CreateAndAdd(this.DataMap.SourceType);

            //build the command
            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(
                this.DataMap,
                MappingDirection.SourceToTarget, aggregate, entity);

            //- Apply the changes from the business object to the data object.
            var result = dataMapCommand.ApplyChanges();

            return result;
        }

        protected void UpdateAggregate(TAggregate aggregate)
        {
            var context = this.GetDbContextInstance();

            var result = this.UpdateAggregate(context, aggregate);

            //- apply the saves
            //context.SaveChanges();

            //we still need to read the keys back out from the context item.
            //result.Items.Copy(MappingDirection.TargetToSource);

            this._actionJackson.Add(() => result.Items.Copy(MappingDirection.TargetToSource));
        }
        private MappingInstructionResult UpdateAggregate(DbContext context, TAggregate aggregate)
        {
            //load a fresh object
            var entity = this.TryFindEntityByAggregate(context, aggregate);

            if (entity == null)
            {
                //cannot update it if its not there.
                throw new Exception("Cant do that their update");
            }

            var dataMapCommandBuilder = new MappingInstructionBuilder();
            var dataMapCommand =
                dataMapCommandBuilder.Build(this.DataMap,
                MappingDirection.SourceToTarget, aggregate, entity);

            var result = dataMapCommand.ApplyChanges();

            result.ItemsDeleted.ForEach(a => context.Set(a.ObjectReceivingChangesType).Remove(a.ObjectReceivingChanges));

            return result;
        }

        protected TAggregate TryFindAggregate(params Object[] keyValues)
        {
            var context = this.GetDbContextInstance();

            return TryFindAggregate(context, keyValues);

        }
        private TAggregate TryFindAggregate(DbContext context, params Object[] keyValues)
        {
            //find the entity object
            var entity = this.TryFindEntity(context, keyValues);

            //exit out early
            if (entity == null)
                return default(TAggregate);
            else
                return this.Hydrate(entity);
        }

        protected Boolean EntityExists(params Object[] keyValues)
        {
            var context = this.GetDbContextInstance();

            return EntityExists(context, keyValues);

        }
        private Boolean EntityExists(DbContext context, params Object[] keyValues)
        {
            //get a handle
            var entitySet = this.EntityDbSet(context);

            //find from data objects...
            var entityObject = entitySet.Find(keyValues);

            //return the entity object
            return entityObject != null;
        }

        protected void DeleteAggregate(TAggregate aggregate)
        {
            //2.- new up a context
            var context = this.GetDbContextInstance();

            var result = this.DeleteAggregate(context, aggregate);

            //-apply changes
            //context.SaveChanges();
        }
        private MappingInstructionResult DeleteAggregate(DbContext context, TAggregate aggregate)
        {
            //find from data objects...
            var entity = this.TryFindEntityByAggregate(context, aggregate);

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
                this.DataMap,
                MappingDirection.SourceToTarget, null, entity);

            var result = dataMapCommand.ApplyChanges();

            result.ItemsDeleted.ForEach(a => context.Set(a.ObjectReceivingChangesType).Remove(a.ObjectReceivingChanges));

            return result;
        }

        #endregion

        #region Public
        private EntityFrameworkRepositoryContext<TDbContext> _context;

        public EntityFrameworkRepositoryContext<TDbContext> Context
        {
            get
            {
                //if (_context == null)
                //{
                //    throw new Exception("Context is not set.");
                //}
                return this._context;
            }
            set
            {
                //yeah,im lazy
                if (this._context != null)
                    throw new Exception("Cannot change the context once it is set");

                this._context = value;

                this._context.ChangesCommitted += ChangesCommitted;
            }
        }



        #endregion
    }

    public interface IRepositoryContext
    {
        void SaveChanges();
        event EventHandler ChangesCommitted;
    }
    public class EntityFrameworkRepositoryContext<TDbContext> : IRepositoryContext, IDisposable
        where TDbContext : DbContext, new()
    {
        public TDbContext DbContext
        {
            get;
            private set;
        }

        public EntityFrameworkRepositoryContext()
        {
            this.DbContext = new TDbContext();
        }
        public EntityFrameworkRepositoryContext(TDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public void SaveChanges()
        {
            var ambientTransaction = System.Transactions.Transaction.Current;

            //save changes..
            this.DbContext.SaveChanges();

            //System.Transactions.Transaction.Current
            if ((ambientTransaction != null) && (ambientTransaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Active))
            {
                ambientTransaction.TransactionCompleted += TransactionCompleted;
            }
            else
            {
                this.OnChangesCommitted();
            }    
        }

        private void TransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            //Console.WriteLine("A transaction has completed:");
            //Console.WriteLine("ID:             {0}", e.Transaction.TransactionInformation.LocalIdentifier);
            //Console.WriteLine("Distributed ID: {0}", e.Transaction.TransactionInformation.DistributedIdentifier);
            //Console.WriteLine("Status:         {0}", e.Transaction.TransactionInformation.Status);
            //Console.WriteLine("IsolationLevel: {0}", e.Transaction.IsolationLevel);

            if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
            {
                this.OnChangesCommitted();
            }
        }
        private void OnChangesCommitted()
        {
            var temp = this.ChangesCommitted;

            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        public event EventHandler ChangesCommitted;

        #region IDisposable
        private bool _disposed = false;

        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    this.DbContext.DisposeIfNotNull();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.

                // Note disposing has been done.
                _disposed = true;

            }
        }
        #endregion
    }

}
