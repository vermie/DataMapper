using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{

    /// <summary>
    /// This is the default cutom type converter. By default, he does nothing. This class is and should remain threadsafe.
    /// </summary>
    internal class NullConverter : ITypeConverter //ahh...the delicious null object pattern!
    {

        private static NullConverter _instance = null;
        public static NullConverter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NullConverter();

                return _instance;
            }
        }

        public object Convert(Type targetType,Type sourceType, object sourceValue)
        {
            if (targetType != sourceType)
            {
                throw new InvalidOperationException(String.Format("The converter '{0}' does not support converting from '{1}' to '{2}'. Verify your mapping is correct or implement your own custom type converter ('{3}').",
                    this.GetType().FullName, sourceType.FullName, targetType.FullName, typeof(ITypeConverter).FullName));
            }

            //do nothing.
            return sourceValue;
        }

    }

    
    
}
