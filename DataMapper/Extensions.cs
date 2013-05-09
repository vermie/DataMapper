using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;
using System.Reflection;

namespace DataMapper
{
    public static class Extensions
    {
        public static Boolean IsOrIsSubclassOf<T>(this Type item)
        {
            return item.IsOrIsSubclassOf<T>();
        }
        public static Boolean IsOrIsSubclassOf(this Type item, Type type)
        {
            if (item == null)
            {
                //could this ever happen with an extension? eh
                throw new ArgumentNullException("item");
            }

            return (item == type) || item.IsSubclassOf(type);
        }

        public static IDataMapperList TryExtractIDataMapperList(this PropertyInfo propertyInfo, Object instance)
        {
            if (instance == null)
            {
                return null;
                //throw new Exception();
            }
            else
            {
                return propertyInfo.ExtractIDataMapperList(instance);
            }
        }
        public static IDataMapperList ExtractIDataMapperList(this PropertyInfo propertyInfo, Object instance)
        {
            var val = propertyInfo.GetValue(instance, null);

            if (val == null)
            {
                throw new DataMapperException("Unable to extract list instance from '{0}' because the property is null.".FormatString(
                    propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name));
            }

            return new DataMapperList(val);
        }

        public static void DisposeIfNotNull(this IDisposable disposable)
        {
            if (disposable != null)
                disposable.Dispose();
        }
    }
}
