using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.EntityFramework.Tests.Specs.Helpers
{
    public class AccountObjectWithExtraProperties
    {

        public String AccountNumber { get; set; }
        public String PolicyNumber { get; set; }
        public int LineOfBusiness { get; set; }
        public DateTime EffectiveDate { get; set; }
        public String StateAbbreviation { get; set; }
        public String PhoneNumber { get; set; }
    }
}
