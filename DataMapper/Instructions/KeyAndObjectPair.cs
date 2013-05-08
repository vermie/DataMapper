using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Instructions
{
    [Serializable()]
    internal class KeyAndObjectPair
    {
        public String Key
        {
            get;
            private set;
        }
        public Object Value
        {
            get;
            private set;
        }

        public KeyAndObjectPair(String key, Object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
