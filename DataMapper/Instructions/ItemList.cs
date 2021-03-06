﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Instructions
{

    [Serializable()]
    public class ItemList : List<Item>
    {
        //public void BuildAndUpdateSourceToTarget(DataMap dataMap, Object source, Object target)
        //{
        //    this.BuildSourceToTarget(dataMap, source, target);
        //    this.UpdateSourceToTarget();
        //}
        //public void BuildAndUpdateTargetToSource(DataMap dataMap, Object source, Object target)
        //{
        //    this.BuildTargetToSource(dataMap, source, target);
        //    this.UpdateTargetToSource();
        //}

        //public void BuildSourceToTarget(DataMap dataMap, Object source, Object target)
        //{
        //    this.Clear();

        //    this.Build(dataMap, source, target, true);
        //}
        //public void BuildTargetToSource(DataMap dataMap, Object source, Object target)
        //{
        //    this.Clear();

        //    this.Build(dataMap, source, target, false);
        //}
        //private void Build(DataMap dataMap, Object source, Object target, Boolean sourceToTarget)
        //{
        //    //this.Clear();

        //    //add this happy dude to our list
        //    this.Add(new ItemUpdate(source, target, dataMap.PropertyMapList));

        //    foreach (var item in dataMap.DataMapCollectionList)
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

        //                this.Build(item.ItemDataMap, collectionItem, newCollectionItem, sourceToTarget);
        //                //item.ItemDataMap.Copy(collectionItem, newCollectionItem, sourceToTarget);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var collectionItem in targetList)
        //            {
        //                var newCollectionItem = item.PropertyMap.SourceCollectionItemType.CreateInstance();

        //                sourceList.Add(newCollectionItem);

        //                this.Build(item.ItemDataMap, newCollectionItem, collectionItem, sourceToTarget);
        //                //item.ItemDataMap.Copy(newCollectionItem, collectionItem, sourceToTarget);
        //            }
        //        }
        //    }
        //}

        public void Copy(MappingDirection mappingDirection)
        {
            this.ForEach(a =>
            {
                if (a.InstructionType != MappingInstructionType.Delete)
                {
                    a.PropertyMapList.MapNonCollection(a.Source, a.Target, mappingDirection);
                }
            });
        }
    }


}
