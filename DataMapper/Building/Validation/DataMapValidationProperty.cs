using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Building
{
    [Serializable()]
    public class DataMapValidationPropertyList : List<DataMapValidationProperty>
    {
        public Boolean IsValid()
        {
            return this.Any(a => a.IsValid == false) == false;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
    [Serializable()]
    public class DataMapValidationProperty
    {
        public String Description
        {
            get;
            internal set;
        }
        public Boolean IsValid
        {
            get;
            internal set;
        }
        public String InvalidReason
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }

}
