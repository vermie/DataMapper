using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Building
{
    public class DataBuilderException : DataMapperException
    {
        public DataMapValidation Validation
        {
            get;
            private set;
        }
        public String ValidationSummary
        {
            get;
            private set;
        }

        public DataBuilderException(DataMapValidation validation)
            :base(BuildErrorMessage(validation))
        {
            this.Validation = validation;
            this.ValidationSummary = validation.BuildValidationErrorMessage();
        }

        private static String BuildErrorMessage(DataMapValidation validation)
        {
            return "Datamap is invalid. One or more errors have been detected with the current datamap schema (see details below). " + Environment.NewLine +
                validation.BuildValidationErrorMessage(true);
        }
    }
}
