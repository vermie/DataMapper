using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;
using DataMapper.Conversion;
using DataMapper.Repositories;
using Theoretical.Data;

namespace Theoretical.Business
{

    public class SimpleRepoThingy : EntityFrameworkSelfRepository<OrderEntity, TheoreticalEntities, Int32>
    {
        protected override void DefineDataMap(DataMapBuilder<OrderEntity, OrderEntity> builder)
        {
            //builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.ThrowException);
            //return;

            //map the odd men out...
            builder
                .MapProperty(a => a.OrderId, b => b.OrderId, MappedPropertyType.KeyField)
                .MapProperty(a => a.OrderItem, b => b.OrderItem)
                .MapProperty(a => a.OrderInformation, b => b.OrderInformation);
            //    .MapProperty(a => a.TaxRate, b => b.TaxRate, MappedPropertyType.Field)
            //    .MapProperty(a => a.Status, b => b.Status, MappedPropertyType.Field, new EnumConverter<StatusEnum>())
            //    .MapProperty(a => a.MyOrderPocoItems, b => b.OrderItem)
            //    .MapProperty(a => a.OrderInformation, b => b.OrderInformation);

            //map some more...
            builder.GetChildMapper<OrderItemEntity, OrderItemEntity>()
                .MapProperty(a => a.OrderItemId, b => b.OrderItemId, MappedPropertyType.KeyField);
            //    .MapProperty(a => a.Upc, b => b.Upc);

            //and even more
            builder.GetChildMapper<OrderInformationEntity, OrderInformationEntity>()
                .MapProperty(a => a.OrderInformationId, b => b.OrderInformationId, MappedPropertyType.KeyField);

            //ignore the errors, allow them a chance to finish mapping
            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.None);
        }
    }

}
