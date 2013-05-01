using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Building
{
    [Serializable()]
    public enum MappingMatchOption
    {
        MatchByPropertyName,
        MatchByPropertyNameAndPropertyType
    }
}
