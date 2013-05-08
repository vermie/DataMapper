using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataMapper.Mapping;

namespace DataMapper.Instructions
{
    public class MappingInstructionBuilder
    {

        public MappingInstruction Build(DataMap dataMap, MappingDirection direction, Object source, Object target)
        {
            this.ValidateTypes(source, target, dataMap);

            return this.BuildCommand(null, dataMap, null, direction, source, target);
        }

        //private void ValidateTypes(Object source, Object target, DataMap dataMap)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    if (target == null)
        //        throw new ArgumentNullException("target");

        //    if (!source.GetType().IsOrIsSubclassOf(dataMap.SourceType))
        //        throw new ArgumentException("source");
        //    if (!target.GetType().IsOrIsSubclassOf(dataMap.TargetType))
        //        throw new ArgumentException("target");
        //}
        private void ValidateTypes(Object source, Object target, DataMap dataMap)
        {
            if ((source != null) && (!source.GetType().IsOrIsSubclassOf(dataMap.SourceType)))
            {
                throw new DataMapperException("source");
            }
            if ((target != null) && (!target.GetType().IsOrIsSubclassOf(dataMap.TargetType)))
            {
                throw new DataMapperException("target");
            }
        }

        private MappingInstruction BuildCommand(MappingInstruction parent, DataMap dataMap, PropertyMap parentCollectionPropertyMap, MappingDirection direction, Object source, Object target)
        {
            //create a new command. This will figure out based on the direction and the source and target objects
            //whether we are doing an add, update, delete or nothing.
            MappingInstruction dataMapCommand = new MappingInstruction(
                parent, parentCollectionPropertyMap, dataMap, direction, source, target);

            //find all the collection properties.
            var collectionProperties = dataMap.DataMapCollectionList.Select(a=>a.PropertyMap).ToList();

            //loop through the collection properties
            foreach (var collectionProperty in collectionProperties)
            {
                var sourceList = collectionProperty.SourcePropertyInfo.TryExtractIDataMapperList(source);
                var targetList = collectionProperty.TargetPropertyInfo.TryExtractIDataMapperList(target);
                var dataMapForCollection = dataMap.FindDataMapForCollectionProperty(collectionProperty);
                var sourceKeyAndObjectList = this.BuildKeyAndObjectPairList(sourceList, dataMapForCollection, true);
                var targetKeyAndObjectList = this.BuildKeyAndObjectPairList(targetList, dataMapForCollection, false);

                //do the full outer join
                var sourceToTargetFullOuterJoin = sourceKeyAndObjectList.FullOuterJoin(
                    targetKeyAndObjectList,
                    x => x.Key,
                    x => x.Key,
                    (x1, x2) => new Tuple<KeyAndObjectPair, KeyAndObjectPair>(x1, x2))
                    .ToList();

                //at this point, we have all the things put together for us
                foreach (var item in sourceToTargetFullOuterJoin)
                {
                    //yummy recursion
                    dataMapCommand.Children.Add(
                        this.BuildCommand(
                        dataMapCommand, 
                        dataMapForCollection, 
                        collectionProperty, 
                        direction, 
                        item.Item1 == null ? null : item.Item1.Value, 
                        item.Item2 == null ? null : item.Item2.Value));
                }
            }

            return dataMapCommand;
        }

        private KeyAndObjectPairList BuildKeyAndObjectPairList(IDataMapperList rawList, DataMap dataMap, Boolean buildForSource)
        {
            KeyAndObjectPairList listy = new KeyAndObjectPairList();

            if (rawList != null)
            {
                foreach (var item in rawList)
                {
                    listy.Add(new KeyAndObjectPair(
                        buildForSource ? dataMap.BuildSourceAggregateKey(item) : dataMap.BuildTargetAggregateKey(item),
                        item));
                }
            }

            return listy;
        }

    }

    
}
