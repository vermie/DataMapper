using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{
    public sealed class EnumConverter<Enumeration> : EnumConverterGenericImplementation
    {
        public EnumConverter() :
            base(typeof(Enumeration))
        {

        }
    }
}
