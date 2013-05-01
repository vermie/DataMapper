using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Commands
{
    [Serializable()]
    public class CommandResult
    {

        public CommandResultSourceTargetPairList SourceTargetPairList
        {
            get;
            private set;
        }
        public CommandResultItemList ItemsAdded
        {
            get;
            private set;
        }
        public CommandResultItemList ItemsDeleted
        {
            get;
            private set;
        }

        public CommandResult()
        {
            this.SourceTargetPairList = new CommandResultSourceTargetPairList();
            this.ItemsAdded = new CommandResultItemList();
            this.ItemsDeleted = new CommandResultItemList();
        }
    }  
}
