using System;
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
    public class OrderDataMapBlock : CrudEntityFrameworkRepository<Order, OrderEntity, TheoreticalEntities, Int32>
    {

    }

    public class OrderPocoDataMapBlock : CrudEntityFrameworkRepository<OrderPoco,OrderEntity,TheoreticalEntities, Int32>
    {
        protected override void DefineDataMap(DataMapBuilder<OrderPoco, OrderEntity> builder)
        {
            //builder.TryMapRemainingByConvention();

            ////map the odd men out...
            builder
                .MapProperty(a => a.MyId, b => b.OrderId, MappedPropertyType.KeyField)
                .MapProperty(a => a.TaxRate, b => b.TaxRate, MappedPropertyType.Field)
                .MapProperty(a => a.RenamedStatus, b => b.Status, MappedPropertyType.Field, new EnumConverter<StatusEnum>())
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
