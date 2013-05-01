using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Building
{
    [Serializable()]
    public class DataMapValidationList : List<DataMapValidation>
    {
        public Boolean IsValidList
        {
            get
            {
                return this.Where(a => a.IsValid == false).Count() == 0;
            }
        }

        public string BuildValidationErrorMessage(Boolean errorsOnly = false)
        {
            String something = String.Empty;

            foreach (var item in this)
            {
                if ((errorsOnly) || (item.IsValid == false))
                {
                    something += item.BuildValidationErrorMessage(errorsOnly);
                    something += Environment.NewLine;
                }
            }

            return something;
        }
    }
    [Serializable()]
    public class DataMapValidation
    {

        public String Description
        {
            get;
            internal set;
        }
        public Boolean IsValid
        {
            get
            {
                var topLevelDude = this.Top();

                return topLevelDude.IsEntireDataMapValid();
            }
        }
        public Boolean IsCurrentValid
        {
            get;
            internal set;
        }
        public String InvalidReason
        {
            get;
            internal set;
        }

        public DataMapValidation Parent
        {
            get;
            internal set;
        }
        public DataMapValidationList Children
        {
            get;
            private set;
        }
        public DataMapValidationPropertyList PropertyMapList
        {
            get;
            private set;
        }
        public DataMapValidationPropertyList PropertyMapListInvalid
        {
            get
            {
                var listy = new DataMapValidationPropertyList();

                listy.AddRange(this.PropertyMapList.Where(a => a.IsValid == false));

                return listy;
            }
        }

        internal DataMapValidation()
        {
            this.Children = new DataMapValidationList();
            this.PropertyMapList = new DataMapValidationPropertyList();
        }

        private Boolean IsEntireDataMapValid()
        {
            if (this.IsCurrentValid == false)
            {
                return false;
            }

            foreach (var child in this.Children)
            {
                if (child.IsEntireDataMapValid() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public DataMapValidation Top()
        {
            DataMapValidation topValidation = this;

            while (topValidation.Parent != null)
                topValidation = topValidation.Parent;

            return topValidation;
        }

        public String BuildValidationErrorMessage(Boolean errorsOnly = false)
        {
            return BuildValidationErrorMessage(this,errorsOnly);
        }


        internal static String BuildValidationErrorMessage(DataMapValidation validation, Boolean errorsOnly = false)
        {
            StringBuilder sb = new StringBuilder();

            BuildValidationErrorMessage(validation, sb, 0, errorsOnly);

            return sb.ToString();
        }
        internal static void BuildValidationErrorMessage(DataMapValidation validation, StringBuilder sb, Int32 depth,Boolean errorsOnly)
        {
            if ((errorsOnly) && (validation.IsEntireDataMapValid()))
            {
                return;
            }

            string frontPaddingMain = GenerateTabs(depth);
            string frontPaddingChild = GenerateTabs(depth + 1);

            if(depth != 0)
                sb.AppendLine();


            sb.AppendLine(frontPaddingMain + validation.Description + " - " + validation.InvalidReason);

            foreach (var item in validation.PropertyMapList)
            {
                if ((errorsOnly == false) || (item.IsValid == false))
                {
                    sb.AppendLine(frontPaddingChild + item.Description);
                }
            }

            foreach (var child in validation.Children)
            {
                BuildValidationErrorMessage(child, sb, depth + 1,errorsOnly);
            }
        }

        internal static string GenerateTabs(Int32 count)
        {
            String tabs = String.Empty;

            for (int i = 0; i < count; i++)
            {
                tabs += "\t";
            }

            return tabs;
        }
    }

}
