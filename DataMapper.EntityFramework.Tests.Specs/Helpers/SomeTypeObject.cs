using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.EntityFramework.Tests.Specs.Helpers
{
    public class SomeTypeObject
    {

        public List<SomeTypeItem> SomeTypeItemList { get; set; }
        public String AccountNumber { get; set; }
        public Byte[] Bytes { get; set; }
        public SomeTypeItem[] ObjectArray { get; set; }
        public String[] StringArray { get; set; }
    }

   
}
