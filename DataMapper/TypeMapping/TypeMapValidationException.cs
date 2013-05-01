using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;

namespace DataMapper.TypeMapping
{
    public class TypeMapValidationException : TypeMapException
    {
        public String ValidationErrors
        {
            get;
            private set;
        }
        public String ValidationFull
        {
            get;
            private set;
        }
        public DataMapValidationList ValidationList
        {
            get;
            private set;
        }
        public List<String> ValidationSummaryList
        {
            get;
            private set;
        }

        public TypeMapValidationException(DataMapValidationList validationList)
            : base(BuildMessage(validationList))
        {
            this.ValidationList = validationList;
            this.ValidationSummaryList = new List<string>();

            foreach (var item in validationList)
            {
                this.ValidationSummaryList.Add(item.BuildValidationErrorMessage());
            }

            this.ValidationFull = validationList.BuildValidationErrorMessage(false);
            this.ValidationErrors = validationList.BuildValidationErrorMessage(true);
        }

        private static String BuildMessage(DataMapValidationList validationList)
        {
            return "The typemap is invalid. One or more typemaps have mapping errors. " + Environment.NewLine +
                validationList.BuildValidationErrorMessage(true);

        }
    }
}
