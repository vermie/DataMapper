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
    
    public partial class OrderInformationEntity
    {
        public int OrderInformationId { get; set; }
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; }
        public Nullable<decimal> TrackAmount { get; set; }
        public Nullable<System.DateTime> TrackDate { get; set; }
    
        public virtual OrderEntity Order { get; set; }
    }
}