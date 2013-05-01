using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Commands
{
    [Serializable()]
    public enum CommandType
    {
        None,
        Create,
        Update,
        Delete
    }
}
