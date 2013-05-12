using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;
using DataMapper.Cache;
using DataMapper.Instructions;
using DataMapper.Mapping;

namespace DataMapper.Repositories
{
    public abstract class Repository<TAggregate,TAggregateEncapsulated>
        where TAggregate: new()//source
        where TAggregateEncapsulated: new()//target
    {
        private DataMap _dataMap=null;
        private String _dataMapCacheKey;

        protected DataMap DataMap
        {
            get
            {
                if(this._dataMap == null)
                {
                    //grab the cached buddy if we have it enabled
                    if (this.CachingEnabled)
                    {
                        this._dataMap = DataMapCache.Instance.TryFind(this._dataMapCacheKey);
                    }

                    if (this._dataMap == null)
                    {
                        this._dataMap = this.BuildDataMap();

                        DataMapCache.Instance.AddItem(this._dataMapCacheKey, this._dataMap);
                    }
                }

                return this._dataMap;
            }
            private set
            {
                this._dataMap = value;
            }
        }

        #region Abstract/virtual

        protected virtual void DefineDataMap(DataMapBuilder<TAggregate, TAggregateEncapsulated> builder)
        {
            //by default we will map everything by convention
            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.ThrowException);
        }

        public Boolean CachingEnabled
        {
            get;
            set;
        }

        #endregion

        protected Repository()
        {
            //set the cache key. A cheap and good way of implementing this is assuming that 
            //the type itself will represent a unique 'Datamap' cache object
            this._dataMapCacheKey = this.GetType().AssemblyQualifiedName;
            this.CachingEnabled = true;
        }


        //set some crap up (like a context or something)
        //leave this to the implementer....

        //define your datamap!
        //this should be static
        private DataMap BuildDataMap()
        {
            //create the builder
            var builder = new DataMapBuilder<TAggregate, TAggregateEncapsulated>();

            //Call into the builder to allow user to build the mapping.
            this.DefineDataMap(builder);

            //Get the finalized map from the databuilder
            return builder.FinalizeMap();  
        }

        //provide access to the 'hydration' method (so that implementers can load stuff and get their objects back)
        protected IEnumerable<TAggregate> Hydrate(IEnumerable<TAggregateEncapsulated> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            List<TAggregate> listy = new List<TAggregate>();

            source.ForEach(a => listy.Add(this.Hydrate(a)));

            return listy;
        }
        protected TAggregate Hydrate(TAggregateEncapsulated aggregateEncapsulated)
        {
            if (aggregateEncapsulated == null)
                throw new ArgumentNullException("aggregateEncapsulated");

            //create a command builder...
            var dataMapCommandBuilder = new MappingInstructionBuilder();

            //build the command which will understand how to create and hydrate a new instance of TAggregate
            var dataMapCommand =
                dataMapCommandBuilder.Build(this.DataMap, MappingDirection.TargetToSource, null, aggregateEncapsulated);

            //applying the changes will do the work of creating the new object
            var result = dataMapCommand.ApplyChanges();

            //get the result, store it
            var aggregate = (TAggregate)dataMapCommand.ObjectReceivingChanges;

            //return the result
            return aggregate;
        }
        

        //allow the add, update, delete methods. Possibly a find method as well.
        protected MappingInstructionResult GetChanges(TAggregate aggregate,TAggregateEncapsulated aggregateEncapsulated)
        {
            //create a command builder...
            var mappingInstructionBuilder = new MappingInstructionBuilder();

            //build the command which will understand how to create and hydrate a new instance of TAggregate
            var mappingInstruction =
                mappingInstructionBuilder.Build(this.DataMap, MappingDirection.SourceToTarget, aggregateEncapsulated, null);

            //applying the changes will do the work of creating the new object
            var result = mappingInstruction.ApplyChanges();

            return result;
        }
    }
}
