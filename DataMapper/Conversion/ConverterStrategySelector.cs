using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Conversion
{

    internal sealed class ConverterStrategySelector
    {

        private static ConverterStrategySelector _instance = null;
        public static ConverterStrategySelector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConverterStrategySelector();
                }

                return _instance;
            }
        }

        private ConverterStrategySelector()
        {

        }

        public ITypeConverter SelectStrategy(Type targetType, Type sourceType)
        {
            ITypeConverter converter;

            //try to use the enum one...
            if (this.TryUseEnumConversion(targetType, sourceType, out converter))
            {
                return converter;
            }

            //default to using the null one
            return NullConverter.Instance;
        }

        private Boolean TryUseEnumConversion(Type targetType, Type sourceType, out ITypeConverter converter)
        {
            converter = null;

            //use enum conversion automatically here if we can
            if ((EnumConverterGenericImplementation.IsEnumTypeAndUnderlyingTypesMatch(sourceType, targetType)) ||
                (EnumConverterGenericImplementation.IsEnumTypeAndUnderlyingTypesMatch(targetType, sourceType)))
            {
                converter = new EnumConverterGenericImplementation(sourceType.IsEnum ? sourceType : targetType);

                return true;
            }

            return false;
        }

    }
}
