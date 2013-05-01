using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.EntityFramework.Tests.Specs.Helpers
{
    public class AccountObject
    {

        public String AccountNumber { get; set; }
        public String PolicyNumber { get; set; }
        public int LineOfBusiness { get; set; }
    }
}
