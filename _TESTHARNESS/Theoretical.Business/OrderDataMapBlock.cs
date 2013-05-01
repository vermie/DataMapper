using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theoretical.Data;
using DataMapper.EntityFramework;
using DataMapper;
using DataMapper.EntityFramework.Poco;
using DataMapper.Conversion;
using DataMapper.Building;

namespace Theoretical.Business
{
    public class OrderDataMapBlock : DataMapEntityRepository<TheoreticalEntities, OrderEntity, Order, Int32>
    {

    }

    public class OrderPocoDataMapBlock : DataMapEntityRepository<TheoreticalEntities, OrderEntity, OrderPoco, Int32>
    {
        protected override void BuildDataMap(DataMapBuilder<OrderEntity, OrderPoco> builder)
        {
            //builder.TryMapRemainingByConvention();

            ////map the odd men out...
            builder
                .MapProperty(a => a.OrderId, b => b.MyId, MappedPropertyType.KeyField)
                .MapProperty(a => a.TaxRate, b => b.TaxRate, MappedPropertyType.Field)
                .MapProperty(a => a.Status, b => b.RenamedStatus, MappedPropertyType.Field, new EnumConverter<StatusEnum>())
                .MapProperty(a => a.OrderItem, b => b.MyOrderPocoItems)
                .MapProperty(a => a.OrderInformation, b => b.OrderInformation);

            ////map some more...
            builder.GetChildMapper<OrderItemEntity, OrderItemPoco>()
                .MapProperty(a => a.OrderItemId, b => b.OrderItemId, MappedPropertyType.KeyField)
                .MapProperty(a => a.Upc, b => b.RenamedUpc);

            //and even more
            builder.GetChildMapper<OrderInformationEntity, OrderInformationPoco>()
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
