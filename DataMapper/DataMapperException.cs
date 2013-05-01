using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper
{

    public class DataMapperException:Exception
    {
        public DataMapperException()
        {

        }
        public DataMapperException(String message)
            : base(message)
        {

        }
        public DataMapperException(String message,Exception innerException)
            : base(message,innerException)
        {

        }
    }

}
