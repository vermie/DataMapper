using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Instructions
{
    [Serializable()]
    public enum MappingInstructionType
    {
        None,
        Create,
        Update,
        Delete
    }
}
