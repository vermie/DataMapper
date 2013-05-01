using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.IO;
using System.Data.Objects;

namespace Theoretical.Data
{
    public static class DBSetExtensions
    {

        public static OrderEntity TryFindOrderEntityByNumber(this TheoreticalEntities context, String orderNumber)
        {
            return context.BuildBaseOrderQuery()
                .Where(a => a.Number == orderNumber)
                .FirstOrDefault();
        }
        public static OrderEntity TryFindOrderEntity(this TheoreticalEntities context, Int32 orderId)
        {
            return context.BuildBaseOrderQuery()
                .Where(a => a.OrderId == orderId)
                .FirstOrDefault();
        }

        internal static IQueryable<OrderEntity> BuildBaseOrderQuery(this TheoreticalEntities context)
        {
            return context.OrderEntity
                .Include(a => a.OrderItem);
        }
    }
}
