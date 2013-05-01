using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class DataMapCollection
    {
        public PropertyMap PropertyMap
        {
            get;
            set;
        }
        public DataMap ItemDataMap
        {
            get;
            set;
        }

        public DataMapCollection(DataMap itemDataMap, PropertyMap propertyMap)
        {
            this.ItemDataMap = itemDataMap;
            this.PropertyMap = propertyMap;
        }
    }
}
