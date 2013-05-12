using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{
    public sealed class EnumConverter<TEnumeration, TValueType>: CustomConverter<TEnumeration,TValueType>
    {
        private Type EnumType
        {
            get
            {
                return typeof(TEnumeration);
            }
        }

        public EnumConverter()
        {
            if (!this.EnumType.IsEnum)
            {
                throw new DataMapperException("Invalid generic parameter used to create class '{0}'. The type supplied '{1}' is NOT an enumeration.".FormatString(
                    this.GetType().FullName, this.EnumType.FullName));
            }
        }

        protected override TEnumeration Convert(TValueType value)
        {
            if (value == null)
            {
                return default(TEnumeration);
            }

            return (TEnumeration)Enum.Parse(this.EnumType, value.ToString());
        }
        protected override TValueType Convert(TEnumeration value)
        {
            return (TValueType)System.Convert.ChangeType(value, typeof(TValueType));
        }

    }
}
