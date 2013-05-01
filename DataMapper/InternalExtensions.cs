using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq.Expressions;
using DataMapper.Building;
using DataMapper.Mapping;

namespace DataMapper
{

    
    internal static class InternalExtensions
    {
        public static DataMapBuilderCore Top(this DataMapBuilderCore dataMapBuilderCore)
        {
            var top = dataMapBuilderCore;

            while (top.Parent != null)
            {
                top = top.Parent;
            }

            return top;
        }
        public static DataMap Top(this DataMap dataMap)
        {
            var top = dataMap;

            while (top.Parent != null)
            {
                top = top.Parent;
            }

            return top;
        }

        public static void AddRange<T>(this ISet<T> iSet, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                iSet.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> iEnumerable, Action<T> action)
        {
            foreach (var item in iEnumerable)
            {
                action(item);
            }
        }


        public static IEnumerable<Tuple<PropertyInfo, PropertyInfo>> MappingFullOuterJoin(
            this IEnumerable<PropertyInfo> sourcePropertyInfoArray, IEnumerable<PropertyInfo> targetPropertyInfoArray)
        {
            return sourcePropertyInfoArray.MappingFullOuterJoin(targetPropertyInfoArray, MappingMatchOption.MatchByPropertyNameAndPropertyType);
        }
        public static IEnumerable<Tuple<PropertyInfo, PropertyInfo>> MappingFullOuterJoin(
            this IEnumerable<PropertyInfo> sourcePropertyInfoArray, IEnumerable<PropertyInfo> targetPropertyInfoArray, MappingMatchOption matchOption)
        {
            var result = sourcePropertyInfoArray.FullOuterJoin(
                targetPropertyInfoArray,
                x => ResolveKey(x, matchOption),
                x => ResolveKey(x, matchOption),
                (x1, x2) => new Tuple<PropertyInfo, PropertyInfo>(x1, x2));

            return result;
        }

        public static IEnumerable<Tuple<PropertyInfo, PropertyInfo>> MappingJoin(
            this IEnumerable<PropertyInfo> sourcePropertyInfoArray, IEnumerable<PropertyInfo> targetPropertyInfoArray)
        {
            return sourcePropertyInfoArray.MappingJoin(targetPropertyInfoArray, MappingMatchOption.MatchByPropertyNameAndPropertyType);
        }
        public static IEnumerable<Tuple<PropertyInfo, PropertyInfo>> MappingJoin(
            this IEnumerable<PropertyInfo> sourcePropertyInfoArray, IEnumerable<PropertyInfo> targetPropertyInfoArray, MappingMatchOption joinOption)
        {
            var result = sourcePropertyInfoArray.Join(
                targetPropertyInfoArray,
                x => ResolveKey(x, joinOption),
                x => ResolveKey(x, joinOption),
                (x1, x2) => new Tuple<PropertyInfo, PropertyInfo>(x1, x2));

            return result;
        }
        private static String ResolveKey(PropertyInfo propertyInfo, MappingMatchOption joinOption)
        {
            return propertyInfo.Name + (joinOption == MappingMatchOption.MatchByPropertyName ? String.Empty : propertyInfo.PropertyType.FullName);
        }

        public static Boolean IsMappingMatch(this PropertyInfo source, PropertyInfo target, MappingMatchOption option)
        {
            switch(option)
            {
                case MappingMatchOption.MatchByPropertyName:
                    return (source.Name == target.Name);
                case MappingMatchOption.MatchByPropertyNameAndPropertyType:
                    return (source.Name == target.Name) && (source.PropertyType == target.PropertyType);
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }


        //public static IEnumerable<PropertyInfo> GetMappingProperties(this Type type)
        //{
        //    BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        //    return type.GetProperties(bindingFlags)
        //        .Where(a=>a.IsPropertyTypeIntegralType() || a.IsPropertyTypeCollectionType());
        //}
        //public static IEnumerable<PropertyInfo> GetMappingIntegralProperties(this IEnumerable<PropertyInfo> propertyInfoArray)
        //{
        //    return propertyInfoArray.Where(a => a.IsPropertyTypeIntegralType() == true).ToList();
        //}
        //public static IEnumerable<PropertyInfo> GetMappingCollectionProperties(this IEnumerable<PropertyInfo> propertyInfoArray)
        //{
        //    return propertyInfoArray.Where(a => a.IsPropertyTypeCollectionType() == true).ToList();
        //}
        //public static IEnumerable<PropertyInfo> GetMappingNonIntegralProperties(this IEnumerable<PropertyInfo> propertyInfoArray)
        //{
        //    return propertyInfoArray.Where(a => a.IsPropertyTypeIntegralType() == false).ToList();
        //}
        public static IEnumerable<PropertyInfo> GetMappingProperties(this Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            return type.GetProperties(bindingFlags);
        }
        

        public static Boolean IsPropertyTypeIntegralType(this PropertyInfo propertyInfo)
        {
            return IsIntegralType(propertyInfo.PropertyType);
        }
        public static Boolean IsIntegralType(this Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return (elementType.IsValueType ||
                        elementType == typeof(String));
            }
            return
                (type.IsValueType) ||
                (type == typeof(String));
        }
        public static Boolean IsPropertyTypeCollectionType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.TryExtractPropertyTypeCollectionItemType() != null;
        }
        public static Boolean IsPropertyTypeCollectionType(this PropertyInfo propertyInfo, IEnumerable<Type> filterList)
        {
            var result = propertyInfo.TryExtractPropertyTypeCollectionItemType();

            if (result != null)
            {
                return filterList.Contains(result);
            }

            return false;
        }   
        public static Type ExtractPropertyTypeCollectionItemType(this PropertyInfo propertyInfo)
        {
            var result = propertyInfo.TryExtractPropertyTypeCollectionItemType();

            if (result == null)
            {
                throw new DataMapperException("Not a collection type?");
            }

            return result;
        }
        public static Type TryExtractPropertyTypeCollectionItemType(this PropertyInfo propertyInfo)
        {
            return TryExtractCollectionItemType(propertyInfo.PropertyType);
        }
        public static Type TryExtractCollectionItemType(this Type type)
        {
            Type iEnumerable = FindIEnumerable(type);

            if (iEnumerable == null)
            {
                return null;
                //return type;
            }
            else
            {
                return iEnumerable.GetGenericArguments()[0];
            }
        }
        #region This code robbed from http://stackoverflow.com/questions/1900353/how-to-get-the-type-contained-in-a-collection-through-reflection
        //public static Boolean PropertyTypeInheritsFromICollection(this PropertyInfo propertyInfo)
        //{
        //    if (propertyInfo.IsPropertyTypeIntegralType())
        //    {
        //        return false;
        //    }

        //    //check to see if its a generic type definition
        //    if (!propertyInfo.PropertyType.IsGenericType)
        //    {
        //        return false;
        //    }

        //    //check to see if its assignable to the generic collection interface
        //    if (!typeof(ICollection<>).IsAssignableFrom(propertyInfo.PropertyType.GetGenericTypeDefinition()))
        //    {
        //        return false;
        //    }

        //    //check for generic arguments = 1?

        //    return true;
        //}

        private static Type FindIEnumerable(Type type)
        {
            if (type == null || type == typeof(string))
                return null;
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var et = elementType.GetType();
                if (elementType.IsValueType || elementType == typeof(string))
                {
                    return null;
                }
                else
                {
                    return typeof (IEnumerable<>).MakeGenericType(type.GetElementType());
                }
            }
            if (type.IsGenericType)
            {
                foreach (Type arg in type.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(type))
                    {
                        return ienum;
                    }
                }
            }
            Type[] interfaces = type.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                foreach (Type iface in interfaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                return FindIEnumerable(type.BaseType);
            }
            return null;
        }
        #endregion

        
        

        public static IEnumerable<PropertyMap> CollectionProperties(this IEnumerable<PropertyMap> propertyMap)
        {
            return propertyMap.Where(a => a.IsCollection);
        }
        public static IEnumerable<PropertyMap> KeyProperties(this IEnumerable<PropertyMap> propertyMap)
        {
            return propertyMap.Where(a => a.IsKey);
        }


        public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner, 
            Func<TOuter, TKey> outerKeySelector, 
            Func<TInner, TKey> innerKeySelector, 
            Func<TOuter, TInner, TResult> resultSelector)
            where TInner : class
            where TOuter : class
        {
            var innerLookup = inner.ToLookup(innerKeySelector);
            var outerLookup = outer.ToLookup(outerKeySelector);

            var innerJoinItems = inner
                .Where(innerItem => !outerLookup.Contains(innerKeySelector(innerItem)))
                .Select(innerItem => resultSelector(null, innerItem));

            return outer
                .SelectMany(outerItem =>
                {
                    var innerItems = innerLookup[outerKeySelector(outerItem)];

                    return innerItems.Any() ? innerItems : new TInner[] { null };
                }, resultSelector)
                .Concat(innerJoinItems);
        }

        public static String FormatString(this String theString, Object arg0)
        {
            return string.Format(theString, arg0);
        }
        public static String FormatString(this String theString, Object arg0, Object arg1)
        {
            return string.Format(theString, arg0,arg1);
        }
        public static String FormatString(this String theString, Object arg0, Object arg1, Object arg2)
        {
            return string.Format(theString, arg0, arg1,arg2);
        }
        public static String FormatString(this String theString, params Object[] parameters)
        {
            return string.Format(theString, parameters);
        }


        /// <summary>
        /// [ <c>public static object GetDefault(Type type)</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <param name="type">The Type for which to get the default value</param>
        /// <returns>The default value for <paramref name="type"/></returns>
        /// <remarks>
        /// If a reference Type, or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault&lt;T&gt;"/>
        public static object GetDefault(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (!type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        }

        public static Object CreateInstance(this Type type)
        {
            return type.Assembly.CreateInstance(type.FullName, false);
        }

    }

    

}
