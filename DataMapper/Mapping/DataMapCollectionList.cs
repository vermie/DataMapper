using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class DataMapCollectionList : List<DataMapCollection>
    {
        public DataMapCollection TryFindByItemDataMap(Type sourceType, Type targetType)
        {
            return this.Where(a =>
                a.ItemDataMap.SourceType == sourceType &&
                a.ItemDataMap.TargetType == targetType).FirstOrDefault();
        }
    }
}
