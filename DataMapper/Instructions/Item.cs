using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Instructions
{

    [Serializable()]
    public class Item
    {
        public Object ObjectReceivingChanges
        {
            get
            {
                if (Direction == MappingDirection.SourceToTarget)
                    return Target;
                else
                    return Source;
            }
        }
        public Type ObjectReceivingChangesType
        {
            get
            {
                if (Direction == MappingDirection.SourceToTarget)
                    return TargetType;
                else
                    return SourceType;
            }
        }

        public MappingDirection Direction
        {
            get;
            private set;
        }
        public MappingInstructionType InstructionType
        {
            get;
            private set;
        }
        public Object Source
        {
            get;
            private set;
        }
        public Type SourceType
        {
            get;
            private set;
        }
        public Object Target
        {
            get;
            private set;
        }
        public Type TargetType
        {
            get;
            private set;
        }
        public PropertyMapCollection PropertyMapList
        {
            get;
            private set;
        }

        public Item(MappingInstructionType instructionType,MappingDirection direction, Object source, Type sourceType, Object target, Type targetType, PropertyMapCollection propertyMapList)
        {
            this.InstructionType = instructionType;
            this.Direction = direction;
            this.Source = source;
            this.SourceType = sourceType;
            this.Target = target;
            this.TargetType = targetType;
            this.PropertyMapList = propertyMapList;
        }

    }

}
