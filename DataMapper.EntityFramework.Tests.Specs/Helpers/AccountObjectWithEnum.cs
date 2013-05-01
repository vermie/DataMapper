using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.EntityFramework.Tests.Specs.Helpers
{
    public class AccountObjectWithEnum
    {

        public String AccountNumber { get; set; }
        public String PolicyNumber { get; set; }
        public LineOfBusiness LineOfBusiness { get; set; }
    }


    public enum LineOfBusiness
    {
        None = 0,
        Auto = 1,
        Property = 2
    }

}


