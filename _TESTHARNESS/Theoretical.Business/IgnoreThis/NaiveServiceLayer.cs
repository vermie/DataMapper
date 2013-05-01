using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theoretical.Data;
using DataMapper;
using DataMapper.EntityFramework;

namespace Theoretical.Business
{

    public class NaiveServiceLayer
    {
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

                //get a newly minted bidness object
                var newBusinessObject = this.CreateNewBusinessObjectsFromDataObjects(freshDataObjectFromDatabase);

                //shovel the data into the business object
                this.Map(freshDataObjectFromDatabase, newBusinessObject);

                //save the memento
                this.TrySaveMemento(newBusinessObject);

                //return the result
                return newBusinessObject;
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
                var orderEntity = this.CreateNewDataObjectsFromBusinessObjects(context, order, StateToCreateObjectsIn.Add);

                //4. - Apply the changes from the business object to the data object.
                this.Map(order, orderEntity);

                //4.- save the changes. we do this because it represents 'one' transaction
                context.SaveChanges();

                //5.- remap any other changes back to the business object? or simply create a new business object and return that?
                this.Map(orderEntity, order);

                //6.- save the memento?
                this.TrySaveMemento(order);
            }
        }
        private void TrySaveMemento(Order order)
        {
            var mementoInterface = order as IMemento;

            if (mementoInterface != null)
            {
                //save the memento...
                mementoInterface.State = order.CopyUsingBinarySerialization<Order>();
            }
        }

        /// <summary>
        /// A 'naive' delete method...
        /// </summary>
        /// <param name="order"></param>
        public void Delete_FailsIfBusinessObjectTamperedWith(Order order)
        {
            //do the validation up front...

            //open up a context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                //get a data object up and running
                var orderEntity = this.CreateNewDataObjectsFromBusinessObjects(context, order, StateToCreateObjectsIn.Unattached);

                //map the changes to the data.
                this.Map(order, orderEntity);

                //reattach it?
                context.OrderEntity.Attach(orderEntity);

                //apply the deletes to the
                this.ApplyDeletes(context, orderEntity);

                //save the deletes
                context.SaveChanges();

                //any post delete processing?
            }
        }

        public void Delete(Order order)
        {
            //open up a context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //try to get the memento..
                var mementoOrCurrentState = this.RetrieveBusinessObjectCompareState(order);

                if (mementoOrCurrentState == null)
                {
                    //throw or return? its already gone...
                    //for now, just return..
                    return;
                }

                //concurrency check here???

                //get a data object up and running...use one based on the state we read?
                var orderEntity = this.CreateNewDataObjectsFromBusinessObjects(context, mementoOrCurrentState, StateToCreateObjectsIn.Unattached);

                //do a concurrency check here? perhaps...
                //concurrency check.
                this.Map(mementoOrCurrentState, orderEntity);

                //attach this 'trusted' state to the context...we will do the deletes based on THIS, not the 
                context.OrderEntity.Attach(orderEntity);

                //apply the deletes to the object.
                this.ApplyDeletes(context, orderEntity);

                //save the deletes
                context.SaveChanges();

                //any post delete processing?
            }
        }


        public void Update_Naive(Order order)
        {
            //open up a context
            using (var context = new Theoretical.Data.TheoreticalEntities())
            {
                //try to get the memento..
                var mementoOrCurrentState = this.RetrieveBusinessObjectCompareState(order);

                if (mementoOrCurrentState == null)
                {
                    //throw or return? its already gone...
                    //for now, just return..
                    return;
                }

                //we should do a concurrency check here??


                //get a data object up and running...use one based on the state we read?
                var orderEntity = this.CreateNewDataObjectsFromBusinessObjects(context, mementoOrCurrentState, StateToCreateObjectsIn.Unattached);
                //copy the data from the old bidness into the new data object
                this.Map(mementoOrCurrentState, orderEntity);

                //attach this 'trusted' state to the context...we will do the deletes based on THIS, not the 
                context.OrderEntity.Attach(orderEntity);

                //apply the deletes to the object.
                this.ApplyChanges(context, orderEntity, order);

                //save the deletes
                context.SaveChanges();

                //remap any other changes back to the business object? or simply create a new business object and return that?
                this.Map(orderEntity, order);

                //save the new memento
                this.TrySaveMemento(order);
            }
        }
        //internal NaiveDataMapActionItemList BuildChangeMap(Order oldState, Order proposedState)
        //{
        //    foreach(var item in pro
        //}


        internal void ApplyChanges(TheoreticalEntities context, OrderEntity orderEntity,Order proposedState)
        {
            //remove any items that are not there anymore on the proposed item...
            foreach (var item in proposedState.OrderItem)
            {

            }
        }

        internal void ApplyDeletes(TheoreticalEntities context, OrderEntity orderEntity)
        {
            //kill the kids...
            context.OrderItemEntity.RemoveAll(orderEntity.OrderItem);

            //now kill pappy...
            context.OrderEntity.Remove(orderEntity);
        }

        #region Blank object creation logic segregated here...

        /// <summary>
        /// Look for implementation of memento..if not there, reload
        /// </summary>
        /// <returns></returns>
        internal Order RetrieveBusinessObjectCompareState(Order orderWithChanges)
        {
            var memento = orderWithChanges as IMemento;

            //no memento? load it from the db now
            if (memento == null)
            {
                return this.TryFind(orderWithChanges.OrderId);
            }
            else
            {
                //return the memento...
                return memento.State as Order;
            }
        }
        internal OrderEntity CreateNewDataObjectsFromBusinessObjects(TheoreticalEntities context, Order order, StateToCreateObjectsIn state)
        {
            OrderEntity orderEntity = context.OrderEntity.Create(state);

            for (Int32 i = 0; i < order.OrderItem.Count; i++)
            {
                orderEntity.OrderItem.Add(context.OrderItemEntity.Create(state));
            }

            

            return orderEntity;
        }
        internal Order CreateNewBusinessObjectsFromDataObjects(OrderEntity orderEntity)
        {
            Order order = new Order();

            for (Int32 i = 0; i < orderEntity.OrderItem.Count; i++)
            {
                order.OrderItem.Add(new OrderItem());
            }

            return order;
        }


        #endregion

        #region Mapping logic segregated here...

        internal void Map(object source, object destination)
        {
            if (source.AsType<Order>() && destination.AsType<OrderEntity>())
            {
                this.MapBusinessToEntity((Order)source, (OrderEntity)destination);
            }
            else if (source.AsType<OrderEntity>() && destination.AsType<Order>())
            {
                this.MapEntityToBusiness((OrderEntity)source, (Order)destination);
            }
            else
            {
                throw new ArgumentException("Invalid arguments passed in for source and/or destination. Objects are the wrong type.");
            }
        }

        internal void MapBusinessToEntity(Order order, OrderEntity orderEntity)
        {
            //1.- datamap the order to the entity
            orderEntity.AccountId = order.AccountId;
            orderEntity.ConcurrencyId = order.ConcurrencyId;
            orderEntity.Number = order.Number;
            orderEntity.OrderDate = order.OrderDate;
            orderEntity.OrderId = order.OrderId;
            orderEntity.Status = order.Status;
            orderEntity.TaxRate = order.TaxRate;

            for (Int32 i = 0; i < orderEntity.OrderItem.Count; i++)
            {
                var oItem = order.OrderItem.ToArray()[i];
                var eItem = orderEntity.OrderItem.ToArray()[i];

                eItem.OrderId = oItem.OrderId;
                eItem.ConcurrencyId = oItem.ConcurrencyId;
                eItem.HasSerialNumber = oItem.HasSerialNumber;
                eItem.OrderItemId = oItem.OrderItemId;
                eItem.SalePrice = oItem.SalePrice;
                eItem.SerialNumber = oItem.SerialNumber;
                eItem.Upc = oItem.Upc;
            }
        }

        internal void MapEntityToBusiness(OrderEntity entity, Order order)
        {
            order.AccountId = entity.AccountId;
            order.ConcurrencyId = entity.ConcurrencyId;
            order.Number = entity.Number;
            order.OrderDate = entity.OrderDate;
            order.OrderId = entity.OrderId;
            order.Status = entity.Status;
            order.TaxRate = entity.TaxRate;


            for (Int32 i = 0; i < entity.OrderItem.Count; i++)
            {
                var oItem = order.OrderItem.ToArray()[i];
                var eItem = entity.OrderItem.ToArray()[i];

                oItem.OrderId = eItem.OrderId;
                oItem.ConcurrencyId = eItem.ConcurrencyId;
                oItem.HasSerialNumber = eItem.HasSerialNumber;
                oItem.OrderItemId = eItem.OrderItemId;
                oItem.SalePrice = eItem.SalePrice;
                oItem.SerialNumber = eItem.SerialNumber;
                oItem.Upc = eItem.Upc;
            }
        }

        #endregion

    }





}
