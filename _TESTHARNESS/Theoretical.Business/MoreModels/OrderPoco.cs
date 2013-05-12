using System;
using System.Collections.Generic;

namespace Theoretical.Business
{
    [Serializable()]
    public class OrderPoco
    {
        public OrderPoco()
        {
            this.MyOrderPocoItems = new List<OrderItemPoco>();
            this.OrderInformation = new List<OrderInformationPoco>();
        }

        public int MyId { get; set; }
        public int AccountId { get; set; }
        public StatusEnum Status { get; set; }
        public string Number { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Decimal TaxRate { get; set; }
        public int ConcurrencyId { get; set; }
        public string OptionalNote { get; set; }
        public Nullable<decimal> OptionalPrice { get; set; }



        //public virtual Account Account { get; set; }
        public virtual ICollection<OrderItemPoco> MyOrderPocoItems { get; set; }
        public virtual ICollection<OrderInformationPoco> OrderInformation { get; set; }
    }

    public enum StatusEnum  
    {
        None=0,
        Sweet,
        Awesome,
        Goodness,
        Giggidy
    }
}
