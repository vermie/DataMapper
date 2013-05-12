using System;
using System.Collections.Generic;

namespace Theoretical.Business
{

    [Serializable()]
    public class Order
    {
        public Order()
        {
            this.OrderItem = new List<OrderItem>();
            this.OrderInformation = new List<OrderInformation>();
            this.MultiKey = new List<MultiKey>();
        }

        public int OrderId { get; set; }
        public int Status { get; set; }
        public string Number { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal TaxRate { get; set; }
        public int ConcurrencyId { get; set; }
        public string OptionalNote { get; set; }
        public Nullable<decimal> OptionalPrice { get; set; }



        //public virtual Account Account { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<OrderInformation> OrderInformation { get; set; }
        public virtual ICollection<MultiKey> MultiKey { get; set; }
    }
}
