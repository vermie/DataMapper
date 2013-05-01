using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Commands
{
    [Serializable()]
    public class CommandResultSourceTargetPair
    {
        public Object Source
        {
            get;
            private set;
        }
        public Object Target
        {
            get;
            private set;
        }
        public PropertyMapCollection PropertyMapList
        {
            get;
            private set;
        }

        public CommandResultSourceTargetPair(Object source, Object target, PropertyMapCollection propertyMapList)
        {
            this.Source = source;
            this.Target = target;
            this.PropertyMapList = propertyMapList;
        }

        public void CopySourceToTarget()
        {
            this.Copy(true);
        }
        public void CopyTargetToSource()
        {
            this.Copy(false);
        }
        internal void Copy(Boolean sourceToTarget)
        {
            //copy my property values first.
            foreach (var item in this.PropertyMapList.Where(a => a.IsCollection == false))
            {
                if (sourceToTarget)
                {
                    item.CopySourceToTarget(this.Source, this.Target);
                }
                else
                {
                    item.CopyTargetToSource(this.Source, this.Target);
                }
            }
        }
    }
}
