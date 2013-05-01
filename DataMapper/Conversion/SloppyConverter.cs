using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{

    /// <summary>
    /// Yeah, not sure what I think of this guy...maybe he will be sent to the gallows
    /// </summary>
    internal sealed class SloppyConverter : ITypeConverter
    {
        private SloppyConverter()
        {

        }

        public object Convert(Type targetType,Type sourceType, object sourceValue)
        {
            //if (value == null)
            //    return null;

            return System.Convert.ChangeType(sourceValue, targetType);
        }
    }

}
