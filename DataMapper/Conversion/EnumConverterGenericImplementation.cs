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
        //public object Convert(Type targetType,Type sourceType, object sourceValue)
        //{
        //    Type enumType = this._enumerationType;
        //    var underlyingEnumType = Enum.GetUnderlyingType(enumType);


        //    if (enumType == targetType)
        //    {
        //        //converting from some other type to the enum
        //        return System.Convert.ChangeType(sourceValue, underlyingEnumType);
        //    }
        //    else if (underlyingEnumType == targetType)
        //    {
        //        //converting from the enum to the actual target type
        //        return System.Convert.ChangeType(sourceValue, targetType);
        //    }
        //    else
        //    {
        //        //we only get here if they are asking us to convert from the enum to a type that is NOT the underlying enum type.
        //        throw new DataMapperException("Unable to convert enum type '{0}' to '{1}' because the underlying type of the enum is '{2}'. Make sure the type of the field being mapped to the enumeration matches the underlying type of the enumeration.".FormatString(
        //            enumType.FullName, targetType.FullName, underlyingEnumType.FullName));
        //    }
        //}

        public object Convert(Type targetType, Type sourceType, object sourceValue)
        {
            if (targetType.IsEnum)
            {
                //we need to convert to the enum
                if (targetType != this._enumerationType)
                {
                    throw new DataMapperException("Unable to convert because the enum type is incorrect. Type received '{0}'. Type expected '{1}'"
                        .FormatString(targetType.FullName, this._enumerationType.FullName));
                }
                var underlyingEnumType = Enum.GetUnderlyingType(this._enumerationType);
                if (sourceType != underlyingEnumType)
                {
                    throw new DataMapperException("Unable to convert enum type '{0}' to '{1}' because the underlying type of the enum is '{2}'. Make sure the type of the field being mapped to the enumeration matches the underlying type of the enumeration.".FormatString(
                        this._enumerationType.FullName, targetType.FullName, underlyingEnumType.FullName));
                }

                //all good. Do this shit
                return Enum.Parse(this._enumerationType, sourceValue.ToString());
                //return System.Convert.ChangeType(sourceValue, targetType);
            }
            else if (sourceType.IsEnum)
            {
                //we need to convert to the enum base type
                //we need to convert to the enum
                if (sourceType != this._enumerationType)
                {
                    throw new DataMapperException("Unable to convert because the enum type is incorrect. Type received '{0}'. Type expected '{1}'"
                        .FormatString(sourceType.FullName, this._enumerationType.FullName));
                }
                var underlyingEnumType = Enum.GetUnderlyingType(this._enumerationType);
                if (targetType != underlyingEnumType)
                {
                    throw new DataMapperException("Unable to convert enum type '{0}' to '{1}' because the underlying type of the enum is '{2}'. Make sure the type of the field being mapped to the enumeration matches the underlying type of the enumeration.".FormatString(
                        this._enumerationType.FullName, targetType.FullName, underlyingEnumType.FullName));
                }

                return System.Convert.ChangeType(sourceValue, targetType);
            }
            else
            {
                throw new DataMapperException("Unable to convert type '{0}' to '{1}' because neither type is the expected enum '{2}'.".FormatString(
                    sourceType.FullName, targetType.FullName, this._enumerationType.FullName));
            }

            //Type enumType = this._enumerationType;
            //var underlyingEnumType = Enum.GetUnderlyingType(enumType);


            //if (enumType == targetType)
            //{
            //    //converting from some other type to the enum
            //    return System.Convert.ChangeType(sourceValue, underlyingEnumType);
            //}
            //else if (underlyingEnumType == targetType)
            //{
            //    //converting from the enum to the actual target type
            //    return System.Convert.ChangeType(sourceValue, targetType);
            //}
            //else
            //{
            //    //we only get here if they are asking us to convert from the enum to a type that is NOT the underlying enum type.
            //    throw new DataMapperException("Unable to convert enum type '{0}' to '{1}' because the underlying type of the enum is '{2}'. Make sure the type of the field being mapped to the enumeration matches the underlying type of the enumeration.".FormatString(
            //        enumType.FullName, targetType.FullName, underlyingEnumType.FullName));
            //}
        }

        
    }
    
}
