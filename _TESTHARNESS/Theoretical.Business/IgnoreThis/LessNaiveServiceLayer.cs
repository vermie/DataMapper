using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theoretical.Data;
using DataMapper;
using System.Data.Entity;
using DataMapper.EntityFramework;
using DataMapper.Instructions;
using DataMapper.Building;
using DataMapper.Mapping;

namespace Theoretical.Business
{
    public class LessNaiveServiceLayer
    {
        //DataMap _businessToEntityDataMap;
        DataMap _entityToBusinessDataMap;

        public LessNaiveServiceLayer()
        {
            DataMapBuilder<OrderEntity, Order> builder = new DataMapBuilder<OrderEntity, Order>();


            //builder.MapProperty(a => a.OrderDate, b => b.OrderDate);
            //builder.MapSourcePropertiesByConvention();
            //builder.GetChildMapper<OrderItemEntity, OrderItem>()
            //    .MapSourcePropertiesByConvention();
            //builder.GetChildMapper<OrderInformationEntity, OrderInformation>()
            //    .MapSourcePropertiesByConvention();

            this._entityToBusinessDataMap = builder.FinalizeMap();
        }

        public void DeleteAll()
        {
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                foreach (var item in (from c in context.OrderEntity.Include(a=>a.OrderItem) select c).ToList())
                {
                    context.OrderItemEntity.RemoveAll<OrderItemEntity>(item.OrderItem);
                    context.OrderEntity.Remove(item);
                }

                context.SaveChanges();
            }
        }

        public Order TryFind(Int32 orderId)
        {
            //new up the context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //find from data objects...
                var freshDataObjectFromDatabase = context.TryFindOrderEntity(orderId);
                //exit out early
                if (freshDataObjectFromDatabase == null)
                    return null;


                //var newBusinessObject = new Order();
                var dataMapCommandBuilder = new MappingInstructionBuilder();
                var dataMapCommand =
                    dataMapCommandBuilder.Build(
                    this._entityToBusinessDataMap,
                    MappingDirection.SourceToTarget, freshDataObjectFromDatabase, null); // newBusinessObject);

                var copyItemList = dataMapCommand.ApplyChanges();

                //return the result
                return dataMapCommand.ObjectReceivingChanges as Order;
            }
        }

        public void Add(Order order)
        {
            //1.- do the validation?
            //TODO for this...
            //things to consider...how modular should the validation be? Validation should be divided into several types maybe?
            //1.- Property validation, needed for proper persistance. This must also be run and always be true. Handled via data annotations?
            //2.- Context based 'Always true' type rules. This might include a rule like 'An order in 'Closed' status should never be updated.
            //3.- Context based validation. Depending on the user action, things that should or should not be allowed maybe? Stuff like
            //updating an order and not including shipping should only be allowed when done as part of a larger transaction (like adding an account)
            //This should represent rules 'Outside' of the scope of the provider.

            //2.- new up a context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //3.- create the new data objects.
                var orderEntity = context.OrderEntity.CreateAndAdd();

                //build the command
                var dataMapCommandBuilder = new MappingInstructionBuilder();
                var dataMapCommand =
                    dataMapCommandBuilder.Build(
                    this._entityToBusinessDataMap,
                    MappingDirection.TargetToSource, orderEntity, order);

                //4. - Apply the changes from the business object to the data object.
                var result = dataMapCommand.ApplyChanges();

                //5.- save the changes. we do this because it represents 'one' transaction
                context.SaveChanges();

                //6.- remap any other changes back to the business object? or simply create a new business object and return that?
                result.Items.Copy(MappingDirection.SourceToTarget);

                //save the memento?


                //var copyItemList = new DataMapInstancePairList();
                //copyItemList.BuildTargetToSource(this._entityToBusinessDataMap, orderEntity, order);

                ////4. - Apply the changes from the business object to the data object.
                //copyItemList.UpdateTargetToSource();

                ////4.- save the changes. we do this because it represents 'one' transaction
                //context.SaveChanges();

                ////5.- remap any other changes back to the business object? or simply create a new business object and return that?
                //copyItemList.UpdateSourceToTarget();

                //6.- save the memento?
                //this.TrySaveMemento(order);
            }
        }

        public void Delete(Order order)
        {
            //new up the context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //find from data objects...
                var freshDataObjectFromDatabase = context.TryFindOrderEntity(order.OrderId);

                //exit out early
                if (freshDataObjectFromDatabase == null)
                {
                    return;
                }

                var dataMapCommandBuilder = new MappingInstructionBuilder();
                var dataMapCommand =
                    dataMapCommandBuilder.Build(
                    this._entityToBusinessDataMap,
                    MappingDirection.TargetToSource, freshDataObjectFromDatabase, null);

                var result = dataMapCommand.ApplyChanges();

                result.ItemsDeleted.ForEach(a => context.Set(a.ObjectReceivingChangesType).Remove(a.ObjectReceivingChanges));
                
                context.SaveChanges();

                //return the result
                return;
            }
        }

        public void Update(Order order)
        {
            //open up a context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //find from data objects...
                var freshDataObjectFromDatabase = context.TryFindOrderEntity(order.OrderId);

                if (freshDataObjectFromDatabase == null)
                {
                    //cannot update it if its not there.
                    throw new Exception("Cant do update");
                }

                var dataMapCommandBuilder = new MappingInstructionBuilder();
                var dataMapCommand = 
                    dataMapCommandBuilder.Build(this._entityToBusinessDataMap, 
                    MappingDirection.TargetToSource, freshDataObjectFromDatabase, order);

                var result = dataMapCommand.ApplyChanges();

                context.SaveChanges();

                //we still need to read the keys back out from the context item.
                result.Items.Copy(MappingDirection.SourceToTarget);
            }
        }
    }
}
