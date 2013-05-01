using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DataMapper.Mapping
{
    
    public interface IDataMapperList:IEnumerable
    {
        void Add(Object item);
        void Remove(Object item);

        Boolean IsNull();
    }
}
