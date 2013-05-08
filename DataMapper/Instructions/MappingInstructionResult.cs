using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Instructions
{
    [Serializable()]
    public class MappingInstructionResult
    {

        public ItemList Items
        {
            get;
            private set;
        }
        public ItemList ItemsAdded
        {
            get
            {
                var listy = new ItemList();

                listy.AddRange(this.Items.Where(a => a.InstructionType == MappingInstructionType.Create).ToArray());

                return listy; 
            }
        }
        public ItemList ItemsDeleted
        {
            get
            {
                var listy = new ItemList();

                listy.AddRange(this.Items.Where(a => a.InstructionType == MappingInstructionType.Delete).ToArray());

                return listy; 
            }
        }
        public ItemList ItemsUpdated
        {
            get
            {
                var listy = new ItemList();

                listy.AddRange(this.Items.Where(a => a.InstructionType == MappingInstructionType.Update).ToArray());

                return listy; 
            }
        }

        public MappingInstructionResult()
        {
            this.Items = new ItemList();
        }

    }  
}
