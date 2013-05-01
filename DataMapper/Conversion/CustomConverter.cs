using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{
    public abstract class CustomConverter<Type1, Type2> : ITypeConverter
    {
        protected abstract Type1 Convert(Type2 value);
        protected abstract Type2 Convert(Type1 value);

        public object Convert(Type targetType,Type sourceType, object sourceValue)
        {
            if (targetType == typeof(Type1))
            {
                return this.Convert((Type2)sourceValue);
            }
            else if (targetType == typeof(Type2))
            {
                return this.Convert((Type1)sourceValue);
            }
            else
            {
                throw new InvalidOperationException("The supplied target type '{0}' is not supported.".FormatString(targetType.FullName));
            }
        }

    }

}
