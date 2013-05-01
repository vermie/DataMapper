using System;
using System.Collections.Generic;

namespace Theoretical.Business
{
    [Serializable()]
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public decimal SalePrice { get; set; }
        public string Upc { get; set; }
        public bool HasSerialNumber { get; set; }
        public string SerialNumber { get; set; }
        public int ConcurrencyId { get; set; }
        //public virtual Order Order { get; set; }
    }
}
