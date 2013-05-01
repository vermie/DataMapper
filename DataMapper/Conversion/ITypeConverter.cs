using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{
    public interface ITypeConverter
    {
        Object Convert(Type targetType,Type sourceType, Object sourceValue);
    }
}
