using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theoretical.Business
{
    public class OrderInformation
    {
        public int OrderInformationId { get; set; }
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; }
        public Nullable<decimal> TrackAmount { get; set; }
        public Nullable<System.DateTime> TrackDate { get; set; }
    }
}
