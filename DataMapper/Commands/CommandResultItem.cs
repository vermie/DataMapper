using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Commands
{
    [Serializable()]
    public class CommandResultItem
    {
        public Type ItemType
        {
            get;
            set;
        }
        public Object Item
        {
            get;
            set;
        }

        public CommandResultItem(Type itemType, Object item)
        {
            this.ItemType = itemType;
            this.Item = item;
        }
    }
}
