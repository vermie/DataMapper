using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections;
using DataMapper.Instructions;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class DataMap
    {
        public DataMap Parent
        {
            get;
            set;
        }
        public PropertyMapCollection PropertyMapList
        {
            get;
            set;
        }
        public Type SourceType
        {
            get;
            set;
        }
        public Type TargetType
        {
            get;
            set;
        }

        public DataMapCollectionList DataMapCollectionList
        {
            get;
            set;
        }

        public DataMap()
        {
            this.PropertyMapList = new PropertyMapCollection();
            this.DataMapCollectionList = new DataMapCollectionList();
        }

        
        public DataMap FindDataMapForCollectionProperty(PropertyMap propertyMap)
        {
            if (propertyMap.IsCollection == false)
                throw new DataMapperException("Property map must be collection type");

            var dataMapForCollection = this.DataMapCollectionList.Where(a =>
                    a.ItemDataMap.TargetType == propertyMap.TargetCollectionItemType &&
                    a.ItemDataMap.SourceType == propertyMap.SourceCollectionItemType).First();

            return dataMapForCollection.ItemDataMap;
        }

        public Boolean AreKeysEqual(Object source, Object target)
        {
            return this.ArePropertiesEqual( true, source, target);
        }
        public Boolean ArePropertiesEqual(Boolean compareKeysOnly, Object source, Object target)
        {
            foreach (var item in this.PropertyMapList)
            {
                if ((item.IsCollection == false) && ((compareKeysOnly == false) || (compareKeysOnly && item.IsKey)))
                {
                    if (item.IsPropertyEqual(source, target) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //public String BuildSourceAggregateKey(Object source)
        //{
        //    return this.BuildAggregateKey(source, true);
        //}
        //public String BuildTargetAggregateKey(Object target)
        //{
        //    return this.BuildAggregateKey(target, false);
        //}
        //private String BuildAggregateKey(Object theObject, Boolean useSource)
        //{
        //    //not a great implementation...we will start with this though

        //    if (theObject == null)
        //        throw new DataMapperException("unable to build aggregatekey for object");

        //    String keyAggregate = String.Empty;
            
        //    foreach (var item in this.PropertyMapList.Where(a => a.IsKey))
        //    {
        //        var propertyInfoToUse = useSource ? item.SourcePropertyInfo : item.TargetPropertyInfo;

        //        var value = propertyInfoToUse.GetValue(theObject, null);

        //        keyAggregate += value.ToString();

        //        ////capture a default value....
        //        ////if the value of a key is equal to the 'default' value of a 
        //        //var defaultValue = propertyInfoToUse.PropertyType.GetDefault();
        //        //var comparer = defaultValue as IComparable;
        //        //if (comparer.CompareTo(value) == 0)
        //        //{
        //        //    //do something that 
        //        //    keyAggregate += value.ToString();
        //        //}
        //        //else
        //        //{
        //        //    keyAggregate += value.ToString();
        //        //}
        //    }

        //    return keyAggregate;
        //}

        public CompositeKey BuildSourceAggregateKey(Object source)
        {
            return this.BuildAggregateKey(source, true);
        }
        public CompositeKey BuildTargetAggregateKey(Object target)
        {
            return this.BuildAggregateKey(target, false);
        }
        private CompositeKey BuildAggregateKey(Object theObject, Boolean useSource)
        {
            //not a great implementation...we will start with this though
            if (theObject == null)
                throw new DataMapperException("unable to build aggregatekey for object");

            CompositeKey key = new CompositeKey();

            //the order matters but it should be preserved, right?
            foreach (var item in this.PropertyMapList.Where(a => a.IsKey))
            {
                var propertyInfoToUse = useSource ? item.SourcePropertyInfo : item.TargetPropertyInfo;

                var value = propertyInfoToUse.GetValue(theObject, null);

                key.AddKey(propertyInfoToUse.PropertyType, value);                
            }

            return key;
        }

        internal List<PropertyMap> GetAllMappedProperties()
        {
            List<PropertyMap> newCollection = new List<PropertyMap>();

            this.PropertyMapList.ForEach(a => newCollection.Add(a));

            this.DataMapCollectionList.ForEach(a => newCollection.Add(a.PropertyMap));

            return newCollection;
        }

        public Boolean HasKeyDefined()
        {
            return this.PropertyMapList.KeyProperties().Count() > 0;
        }

        //private void Copy(Object source, Object target, Boolean sourceToTarget)
        //{
        //    //copy my property values first.
        //    foreach (var item in this.PropertyMapList.Where(a => a.IsCollection == false))
        //    {
        //        if (sourceToTarget)
        //        {
        //            item.CopySourceToTarget(source, target);
        //        }
        //        else
        //        {
        //            item.CopyTargetToSource(source, target);
        //        }
        //    }

        //    foreach (var item in this.DataMapCollectionList)
        //    {
        //        IDataMapperList sourceList = item.PropertyMap.SourcePropertyInfo.ExtractIDataMapperList(source);
        //        IDataMapperList targetList = item.PropertyMap.TargetPropertyInfo.ExtractIDataMapperList(target);

        //        if (sourceToTarget)
        //        {
        //            //for each item in the list
        //            foreach (var collectionItem in sourceList)
        //            {
        //                var newCollectionItem = item.PropertyMap.TargetCollectionItemType.CreateInstance();

        //                targetList.Add(newCollectionItem);

        //                item.ItemDataMap.Copy(collectionItem, newCollectionItem, sourceToTarget);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var collectionItem in targetList)
        //            {
        //                var newCollectionItem = item.PropertyMap.SourceCollectionItemType.CreateInstance();

        //                sourceList.Add(newCollectionItem);

        //                item.ItemDataMap.Copy(newCollectionItem, collectionItem, sourceToTarget);
        //            }
        //        }
        //    }
        //}
        //public Object CopySourceToNewTarget(Object source)
        //{
        //    var newObj = this.TargetType.CreateInstance();

        //    this.Copy(source, newObj , true);

        //    return newObj;
        //}
        //public Object CopyTargetToNewSource(Object target)
        //{
        //    var newObj = this.SourceType.CreateInstance();

        //    this.Copy(newObj, target, false);

        //    return newObj;
        //}
        //public void CopySourceToTarget(Object source, Object target)
        //{
        //    this.Copy(source, target, true);
        //}
        //public void CopyTargetToSource(Object source, Object target)
        //{
        //    this.Copy(source, target, false);
        //}
        //private void Copy(Object source, Object target, Boolean sourceToTarget)
        //{
        //    //copy my property values first.
        //    foreach (var item in this.PropertyMapList.Where(a => a.IsCollection == false))
        //    {
        //        if (sourceToTarget)
        //        {
        //            item.CopySourceToTarget(source, target);
        //        }
        //        else
        //        {
        //            item.CopyTargetToSource(source, target);
        //        }
        //    }

        //    foreach (var item in this.DataMapCollectionList)
        //    {
        //        IDataMapperList sourceList = item.PropertyMap.SourcePropertyInfo.ExtractIDataMapperList(source);
        //        IDataMapperList targetList = item.PropertyMap.TargetPropertyInfo.ExtractIDataMapperList(target);

        //        if(sourceToTarget)
        //        {
        //            //for each item in the list
        //            foreach (var collectionItem in sourceList)
        //            {
        //                var newCollectionItem = item.PropertyMap.TargetCollectionItemType.CreateInstance();
                        
        //                targetList.Add(newCollectionItem);

        //                item.ItemDataMap.Copy(collectionItem, newCollectionItem, sourceToTarget);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var collectionItem in targetList)
        //            {
        //                var newCollectionItem = item.PropertyMap.SourceCollectionItemType.CreateInstance();

        //                sourceList.Add(newCollectionItem);

        //                item.ItemDataMap.Copy(newCollectionItem, collectionItem, sourceToTarget);
        //            }
        //        }       
        //    }
        //}

        public override string ToString()
        {
            return "source:{0}->target;{1}".FormatString(
                this.SourceType != null? this.SourceType.FullName:" ",
                this.TargetType != null ? this.TargetType.FullName : " ");
        }
    }
    
    


    
}
