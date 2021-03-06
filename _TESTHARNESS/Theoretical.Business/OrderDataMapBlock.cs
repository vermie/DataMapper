﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theoretical.Data;
using DataMapper;
using DataMapper.Conversion;
using DataMapper.Building;
using DataMapper.Repositories;

namespace Theoretical.Business
{
    public class OrderDataMapBlock : EntityFrameworkCrudRepository<Order, OrderEntity, TheoreticalEntities, Int32>
    {
        protected override void DefineDataMap(DataMapBuilder<Order, OrderEntity> builder)
        {
            //builder.MapProperty(a => a.OrderItem, b => b.OrderItem);
            builder
                .MapProperty(a => a.MultiKey, b => b.MultiKey);

            builder.GetChildMapper<MultiKey, MultiKeyEntity>()
                .MapProperty(a => a.KeyOne, b => b.KeyOne, MappedPropertyType.KeyField, null)
                .MapProperty(a => a.KeyTwo, b => b.KeyTwo, MappedPropertyType.KeyField, null)
                .MapProperty(a => a.KeyThree, b => b.KeyThree, MappedPropertyType.KeyField, null);

            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.ThrowException);
        }

        public void TouchDataMap()
        {
            var yo = this.DataMap;
        }
    }

    public class OrderPocoDataMapBlock : EntityFrameworkCrudRepository<OrderPoco,OrderEntity,TheoreticalEntities, Int32>
    {
        public OrderPoco FindFirst()
        {
            var id = this.Context.DbContext.OrderEntity.First();

            return this.TryFind(id.OrderId);
        }

        protected override void DefineDataMap(DataMapBuilder<OrderPoco, OrderEntity> builder)
        {
            //builder.TryMapRemainingByConvention();

            ////map the odd men out...
            builder
                .MapProperty(a => a.MyId, b => b.OrderId, MappedPropertyType.KeyField)
                .MapProperty(a => a.TaxRate, b => b.TaxRate, MappedPropertyType.Field)
                //.MapProperty(a => a.Status, b => b.Status, MappedPropertyType.Field, new EnumConverter<StatusEnum,Int32>())
                .MapProperty(a => a.MyOrderPocoItems, b => b.OrderItem)
                .MapProperty(a => a.OrderInformation, b => b.OrderInformation);

            ////map some more...
            builder.GetChildMapper<OrderItemPoco, OrderItemEntity>()
                .MapProperty(a => a.OrderItemId, b => b.OrderItemId, MappedPropertyType.KeyField)
                .MapProperty(a => a.RenamedUpc, b => b.Upc);

            //and even more
            builder.GetChildMapper<OrderInformationPoco, OrderInformationEntity>()
                .MapProperty(a => a.OrderInformationId, b => b.OrderInformationId, MappedPropertyType.KeyField);

            //ignore the errors, allow them a chance to finish mapping
            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.None);
        }

    }

    public class MyFunkyConverter : CustomConverter<Nullable<Int32>, Int32>
    {

        protected override int? Convert(int value)
        {
            return new int();
        }

        protected override int Convert(int? value)
        {
            return new int();
        }
    }
}
