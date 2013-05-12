using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theoretical.Business
{
    
    [Serializable()]
    public class MultiKeyPoco
    {
        public int KeyOne { get; set; }
        public System.Guid KeyTwo { get; set; }
        public string KeyThree { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int ConcurrencyId { get; set; }
        public int OrderId { get; set; }
    }
}
