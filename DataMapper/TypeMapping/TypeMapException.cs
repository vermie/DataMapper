using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.TypeMapping
{
    public class TypeMapException : DataMapperException
    {
        public TypeMapException()
        {

        }
        public TypeMapException(String message)
            : base(message)
        {

        }
        public TypeMapException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
