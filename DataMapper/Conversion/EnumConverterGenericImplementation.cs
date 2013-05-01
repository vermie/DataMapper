using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Conversion
{

    public class EnumConverterGenericImplementation : ITypeConverter
    {
        private Type _enumerationType;

        public EnumConverterGenericImplementation(Type enumerationType)
        {
            this._enumerationType = enumerationType;

            if (!this._enumerationType.IsEnum)
            {
                throw new DataMapperException("Invalid generic parameter used to create class '{0}'. The type supplied '{1}' is NOT an enumeration.".FormatString(
                    this.GetType().FullName, this._enumerationType.FullName));
            }
        }
        public object Convert(Type targetType,Type sourceType, object sourceValue)
        {
            Type enumType = this._enumerationType;
            var underlyingEnumType = Enum.GetUnderlyingType(enumType);


            if (enumType == targetType)
            {
                //converting from some other type to the enum
                return System.Convert.ChangeType(sourceValue, underlyingEnumType);
            }
            else if (underlyingEnumType == targetType)
            {
                //converting from the enum to the actual target type
                return System.Convert.ChangeType(sourceValue, targetType);
            }
            else
            {
                //we only get here if they are asking us to convert from the enum to a type that is NOT the underlying enum type.
                throw new DataMapperException("Unable to convert enum type '{0}' to '{1}' because the underlying type of the enum is '{2}'. Make sure the type of the field being mapped to the enumeration matches the underlying type of the enumeration.".FormatString(
                    enumType.FullName, targetType.FullName, underlyingEnumType.FullName));
            }
        }

        internal static Boolean IsEnumTypeAndUnderlyingTypesMatch(Type enumType, Type targetType)
        {
            if (enumType.IsEnum)
            {
                return UnderlyingTypesMatch(enumType, targetType);
            }

            return false;
        }
        internal static Boolean UnderlyingTypesMatch(Type enumType, Type targetType)
        {
            return Enum.GetUnderlyingType(enumType) == targetType;
        }
    }
    
}
