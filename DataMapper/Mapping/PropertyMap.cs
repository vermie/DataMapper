using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using DataMapper.Conversion;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class PropertyMap
    {
        
        public ITypeConverter TypeConverter
        {
            get;
            internal set;
        }

        public PropertyInfo SourcePropertyInfo
        {
            get;
            private set;
        }
        public PropertyInfo TargetPropertyInfo
        {
            get;
            private set;
        }
        public Boolean IsKey
        {
            get;
            private set;
        }
        public Boolean IsCollection
        {
            get;
            private set;
        }
        public Type SourceCollectionItemType
        {
            get;
            internal set;
        }
        public Type TargetCollectionItemType
        {
            get;
            internal set;
        }

        public PropertyMap(PropertyInfo sourcePropertyInfo,PropertyInfo targetPropertyInfo,Boolean isKey,Boolean isCollection)
        {
            this.SourcePropertyInfo = sourcePropertyInfo;
            this.TargetPropertyInfo = targetPropertyInfo;
            this.IsKey = isKey;
            this.IsCollection = isCollection;

            this.TypeConverter = NullConverter.Instance;
        }

        public void Copy(Object source, Object target, MappingDirection mappingDirection)
        {
            if (mappingDirection == MappingDirection.SourceToTarget)
                this.CopySourceToTarget(source, target);
            else
                this.CopyTargetToSource(source, target);
        }
        private void CopySourceToTarget(Object source, Object target)
        {         
            var sourceRawValue = this.SourcePropertyInfo.GetValue(source, null);

            sourceRawValue = this.TypeConverter.Convert(this.TargetPropertyInfo.PropertyType,this.SourcePropertyInfo.PropertyType, sourceRawValue);

            if (this.SourcePropertyInfo.PropertyType.IsArray)
            {
                Array sourceArray = (Array)this.SourcePropertyInfo.GetValue(source);
                var targetType = this.TargetPropertyInfo.PropertyType.GetElementType();
                var targetArray = Array.CreateInstance(targetType, sourceArray.Length);
                Array.Copy(sourceArray, targetArray, sourceArray.Length);
                this.TargetPropertyInfo.SetValue(target, targetArray);
            }
            else
            {
                this.TargetPropertyInfo.SetValue(target, sourceRawValue, null);    
            }
        }
        private void CopyTargetToSource(Object source, Object target)
        {
            var targetRawValue = this.TargetPropertyInfo.GetValue(target, null);

            targetRawValue = this.TypeConverter.Convert(this.SourcePropertyInfo.PropertyType,this.TargetPropertyInfo.PropertyType, targetRawValue);

            if (this.TargetPropertyInfo.PropertyType.IsArray)
            {
                Array targetArray = (Array)this.TargetPropertyInfo.GetValue(target);
                var sourceType = this.SourcePropertyInfo.PropertyType.GetElementType();
                var sourceArray = Array.CreateInstance(sourceType, targetArray.Length);
                Array.Copy(targetArray, sourceArray, targetArray.Length);
                this.SourcePropertyInfo.SetValue(source, sourceArray);
            }
            else
            {
                this.SourcePropertyInfo.SetValue(source, targetRawValue, null);
            }

            
        }
        public Boolean IsPropertyEqual(Object source, Object target)
        {
            var sourceRawValue = this.SourcePropertyInfo.GetValue(source, null);
            var targetRawValue = this.TargetPropertyInfo.GetValue(target, null);


            //for arrays (think byte arrays) we should do something special. For now, we just punt on the issue
            if (this.SourcePropertyInfo.PropertyType.IsArray)
            {
                //or we can just return false???

                //or throw a 'not-implemented'
                throw new DataMapperException("Arrays are not implemented yet");
            }
            else
            {
                return AreIntegralValuesEqual(sourceRawValue, targetRawValue);
            }
        }
        private Boolean AreIntegralValuesEqual(Object sourceRawValue, Object targetRawValue)
        {
            //we have a single struct value or string or nullable type (still struct)
            if (sourceRawValue == null || targetRawValue == null)
            {
                //when one or more of these is null, we must apply special logic.
                //we know that at least one of them is null. In this situation,
                //unless they are both null, this is not equal. nuff said.
                return ((sourceRawValue == null) && (targetRawValue == null));
            }
            else
            {
                //when we have non-null values, the easiest way to do this is to leverage IComparable
                var sourceComparable = sourceRawValue as IComparable;
                //IEquatable<Int32> 

                if (sourceComparable == null)
                {
                    throw new DataMapperException(
                        "Unable to perform comparison on integral property because it does not implement IComparable. Please make the type '{0}' implement this interface."
                        .FormatString(sourceRawValue.GetType().Name));
                }

                //do a convert here
                targetRawValue = this.TypeConverter.Convert(this.SourcePropertyInfo.PropertyType,this.TargetPropertyInfo.PropertyType, targetRawValue);

                return sourceComparable.CompareTo(targetRawValue) == 0;
            }
        }


        public Boolean ContainsPropertyInfoSourceOrTarget(params PropertyInfo[] propertyInfoArray)
        {
            foreach (var item in propertyInfoArray)
            {
                if (this.SourcePropertyInfo == item || this.TargetPropertyInfo == item)
                {
                    return true;
                }
            }

            return false;
        }


        public Boolean IsValid()
        {
            String errorMessage;

            return this.IsValid(out errorMessage);
        }
        public Boolean IsValid(out String errorMessage)
        {
            
            //make sure we gots no nulls
            if (this.SourcePropertyInfo == null)
            {
                errorMessage = "The property map {0} is invalid because the SourcePropertyInfo is null.".FormatString(this.ToString());
                return false;
            }
            if (this.TargetPropertyInfo == null)
            {
                errorMessage = "The property map {0} is invalid because the TargetPropertyInfo is null.".FormatString(this.ToString());
                return false;
            }

            //basic error information
            errorMessage = "The property map {0} is not valid. ".FormatString(this.ToString());

            if (this.IsCollection)
            {
                //maybe do some more checks?
                if (this.SourceCollectionItemType == null)
                {
                    errorMessage += "The property map {0} is specified as a Collection property but the collection item type for the source property could not be determined.".FormatString(this.ToString()) +
                        " Make sure the collection is compatible with the DataMapper supported collection types(IList,IHashSet).";
                    return false;
                }
                if (this.TargetCollectionItemType == null)
                {
                    errorMessage += "The property map {0} is specified as a Collection property but the collection item type for the target property could not be determined.".FormatString(this.ToString()) +
                        " Make sure the collection is compatible with the DataMapper supported collection types(IList,IHashSet).";
                    return false;
                }

                if (this.SourcePropertyInfo.IsPropertyTypeCollectionType() == false)
                {
                    errorMessage += "The property map {0} is specified as a Collection property map but the source property is not a collection.".FormatString(this.ToString());
                    return false;
                }
                if (this.TargetPropertyInfo.IsPropertyTypeCollectionType() == false)
                {
                    errorMessage += "The property map {0} is specified as a Collection property map but the target property is not a collection.".FormatString(this.ToString());
                    return false;
                }
            }
            else
            {
                //ok, not a collection, lets check if we can actually parse it (using the assigned converter)
                if (this.IsValidTypeConverter() == false)
                {
                    errorMessage += "Property map must define a type converter.TypeConverter cannot be null.";
                    return false;
                }
                //not sure if we need this check with the option to do custom type converters...


            }

            if ((this.IsCollection) && (this.IsKey))
            {
                errorMessage += "The property map {0} is specified as both a key and and a collection. This is not valid.".FormatString(this.ToString());
                return false;
            }

            errorMessage = "Property map is valid";
            return true;
        }
        private Boolean IsValidTypeConverter()
        {
            return this.TypeConverter != null;
            //decided against this since its flimsy anyways. for now, valid means it is assigned.
            //try
            //{
            //    //make some special exceptions for string since it does not have a default constructor (wtf?)
            //    var sourceInstance = this.SourcePropertyInfo.PropertyType == typeof(String)? 
            //        String.Empty: this.SourcePropertyInfo.PropertyType.Assembly.CreateInstance(this.SourcePropertyInfo.PropertyType.FullName);
            //    var targetInstance = this.TargetPropertyInfo.PropertyType == typeof(String)?
            //        String.Empty: this.TargetPropertyInfo.PropertyType.Assembly.CreateInstance(this.TargetPropertyInfo.PropertyType.FullName);

            //    var sourceConvertedToTarget = this.CustomTypeConverter.Convert(this.TargetPropertyInfo.PropertyType,this.SourcePropertyInfo.PropertyType, sourceInstance);

            //    var targetConvertedToSource = this.CustomTypeConverter.Convert(this.SourcePropertyInfo.PropertyType,this.TargetPropertyInfo.PropertyType, targetInstance);
            //}
            //catch
            //{
            //    //do nothing here? Just assume a catch is bad
            //    return false;
            //}

            //return true;
        }

        public override string ToString()
        {
            return "'{0}'<->'{1}'".FormatString(
                this.SourcePropertyInfo == null? " ":this.SourcePropertyInfo.DeclaringType.FullName + "." + this.SourcePropertyInfo.Name,
                this.TargetPropertyInfo == null ? " " : this.TargetPropertyInfo.DeclaringType.FullName + "." + this.TargetPropertyInfo.Name);
        }
        public String ToShortString()
        {
            return "'{0}'<->'{1}'".FormatString(
                this.SourcePropertyInfo == null ? " " : this.SourcePropertyInfo.Name,
                this.TargetPropertyInfo == null ? " " : this.TargetPropertyInfo.Name);
        }
    }

}
