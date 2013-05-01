//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Theoretical.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderEntity
    {
        public OrderEntity()
        {
            this.OrderInformation = new HashSet<OrderInformationEntity>();
            this.OrderItem = new HashSet<OrderItemEntity>();
        }
    
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public int Status { get; set; }
        public string Number { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal TaxRate { get; set; }
        public int ConcurrencyId { get; set; }
        public string OptionalNote { get; set; }
        public Nullable<decimal> OptionalPrice { get; set; }
    
        public virtual ICollection<OrderInformationEntity> OrderInformation { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItem { get; set; }
    }
}
