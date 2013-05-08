using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;
using DataMapper.Instructions;
using DataMapper.Mapping;

namespace DataMapper.Repositories
{
    public abstract class Repository<TAggregate,TAggregateEncapsulated>
        where TAggregate: new()
        where TAggregateEncapsulated: new()
    {
        private DataMap _dataMap=null;

        private DataMap DataMap
        {
            get
            {
                if(this._dataMap == null)
                {
                    this._dataMap = this.BuildDataMap();
                }

                return this._dataMap;
            }
            set
            {
                this._dataMap = value;
            }
        }

        #region Abstract/virtual

        protected virtual void BuildDataMap(DataMapBuilder<TAggregate, TAggregateEncapsulated> builder)
        {
            //by default we will map everything by convention
            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.ThrowException);
        }

        #endregion


        //set some crap up (like a context or something)
        //leave this to the implementer....

        //define your datamap!
        //this should be static
        private DataMap BuildDataMap()
        {
            //create the builder
            var builder = new DataMapBuilder<TAggregate, TAggregateEncapsulated>();

            //Call into the builder to allow user to build the mapping.
            this.BuildDataMap(builder);

            //Get the finalized map from the databuilder
            return builder.FinalizeMap();  
        }

        //provide access to the 'hydration' method (so that implementers can load stuff and get their objects back)
        private IEnumerable<TAggregate> Hydrate(IEnumerable<TAggregateEncapsulated> source)
        {
            List<TAggregate> listy = new List<TAggregate>();

            source.ForEach(a => listy.Add(this.Hydrate(a)));

            return listy;
        }
        private TAggregate Hydrate(TAggregateEncapsulated source)
        {
            //create a command builder...
            var dataMapCommandBuilder = new MappingInstructionBuilder();

            //build the command which will understand how to create and hydrate a new instance of TAggregate
            var dataMapCommand =
                dataMapCommandBuilder.Build(this.DataMap, MappingDirection.SourceToTarget, source, null); 

            //applying the changes will do the work of creating the new object
            var result = dataMapCommand.ApplyChanges();

            //get the result, store it
            var aggregate = (TAggregate)dataMapCommand.ObjectReceivingChanges;

            //call post hydrate?
            this.PostHydrate(aggregate, source);

            //return the result
            return aggregate;
        }
        protected virtual void PostHydrate(TAggregate aggregate,TAggregateEncapsulated aggregateEncapsulated)
        {
            //by default do nothing.
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
