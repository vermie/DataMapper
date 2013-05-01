using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper.Mapping;

namespace DataMapper.Building
{

    internal class DataMapValidationBuilder
    {
        private DataMapBuilderCore _dataMapBuilderCore = null;

        public DataMapValidationOptions Options
        {
            get;
            set;
        }
        public DataMapValidationBuilder(DataMapBuilderCore dataMapBuilderCore)
        {
            if (dataMapBuilderCore == null)
                throw new ArgumentNullException("dataMapBuilderCore");

            this._dataMapBuilderCore = dataMapBuilderCore;
            this.Options = new DataMapValidationOptions(dataMapBuilderCore.ValidationOptions);
            //this.Options = DataMapValidationOptions.Default;
        }

        private DataMap DataMap
        {
            get
            {
                return this._dataMapBuilderCore.DataMap;
            }
        }

        public DataMapValidation Build()
        {
            DataMapValidation validation = new DataMapValidation()
            {
                Parent = null
            };

            this.Validate(validation);

            return validation;
        }
        internal void Validate(DataMapValidation validation)
        {
            validation.PropertyMapList.AddRange(this.CreateDataMapValidationPropertyList());

            validation.IsCurrentValid =
                ((this.Options.ValidateKeys == false) || (this.DataMap.HasKeyDefined())) &&
                this.AllSourcePropertiesMapped() &&
                validation.PropertyMapList.IsValid();

            validation.Description = String.Format(
                "'{0}'->'{1}' ({2})",
                this.DataMap.SourceType.FullName,
                this.DataMap.TargetType.FullName,
                validation.IsCurrentValid ? "Valid" : "Invalid");

            if (validation.IsCurrentValid == false)
            {
                if ((this.Options.ValidateKeys) && (this.DataMap.HasKeyDefined() == false))
                    validation.InvalidReason += "{No keys defined}";
                if (this.AllSourcePropertiesMapped() == false)
                    validation.InvalidReason += "{All source properties not mapped}";
                if (validation.PropertyMapList.IsValid() == false)
                    validation.InvalidReason += "{One or more validation properties invalid}";

                validation.InvalidReason = validation.InvalidReason.Trim();
            }

            //recurse into the kids
            foreach (var childMap in this.DataMap.DataMapCollectionList)
            {
                //get a child mapper
                var childMapper = this._dataMapBuilderCore.GetChildMapper(childMap.ItemDataMap.SourceType, childMap.ItemDataMap.TargetType);
                childMapper.ValidationOptions = this.Options;
                var childBuilder = new DataMapValidationBuilder(childMapper);
                var newValidation = new DataMapValidation();

                newValidation.Parent = validation;
                validation.Children.Add(newValidation);

                childBuilder.Validate(newValidation);
            }
        }

        private Boolean AllSourcePropertiesMapped()
        {
            return (this.DataMap.PropertyMapList.Count + this.DataMap.DataMapCollectionList.Count == this._dataMapBuilderCore.SourcePropertyInfoHashSet.Count - this._dataMapBuilderCore.SourceIgnoredPropertyInfoHashSet.Count);
        }
        private DataMapValidationPropertyList CreateDataMapValidationPropertyList()
        {
            DataMapValidationPropertyList listy = new DataMapValidationPropertyList();

            foreach (var sourcePropertyInfo in this._dataMapBuilderCore.SourcePropertyInfoHashSet)
            {
                if (this._dataMapBuilderCore.SourceIgnoredPropertyInfoHashSet.Contains(sourcePropertyInfo))
                {
                    //is it ignored or not?
                    listy.Add(new DataMapValidationProperty()
                    {
                        Description = String.Format("'{0}' -> ? (Ignored)", sourcePropertyInfo.Name),
                        InvalidReason = String.Empty,
                        IsValid = true
                    });
                }
                else
                {
                    String invalidReason = String.Empty;
                    var nonCollectionPropertyMap = this.DataMap.PropertyMapList.Where(a => a.SourcePropertyInfo == sourcePropertyInfo || a.TargetPropertyInfo == sourcePropertyInfo).FirstOrDefault();
                    var collectionItem = this.DataMap.DataMapCollectionList.Where(a => a.PropertyMap.SourcePropertyInfo == sourcePropertyInfo || a.PropertyMap.TargetPropertyInfo == sourcePropertyInfo).FirstOrDefault();
                    DataMapValidationProperty propertyValidationResult = null;
                    Boolean addInvalidReason = true;

                    if (nonCollectionPropertyMap != null)
                    {
                        var isValid = nonCollectionPropertyMap.IsValid(out invalidReason);

                        propertyValidationResult = new DataMapValidationProperty()
                        {
                            Description = String.Format("{0}", nonCollectionPropertyMap.ToShortString()),
                            InvalidReason = invalidReason,
                            IsValid = isValid
                        };

                        if (nonCollectionPropertyMap.IsKey)
                            propertyValidationResult.Description += " (Key)";
                    }
                    else if (collectionItem != null)
                    {
                        var isValid = collectionItem.PropertyMap.IsValid(out invalidReason);

                        var cDescription = String.Format("{0} (Collection)",
                            collectionItem.PropertyMap.ToShortString());


                        //is it ignored or not?
                        propertyValidationResult = new DataMapValidationProperty()
                        {
                            Description = cDescription,
                            InvalidReason = invalidReason,
                            IsValid = isValid
                        };
                    }
                    else
                    {
                        //is it ignored or not?
                        propertyValidationResult = new DataMapValidationProperty()
                        {
                            Description = String.Format("'{0}' -> ?", sourcePropertyInfo.Name),
                            InvalidReason = "The property is not mapped. To ignore this property, mark it as ignored.",
                            IsValid = false
                        };

                        addInvalidReason = false;
                    }

                    if ((propertyValidationResult.IsValid == false) &&
                        (addInvalidReason == true))
                    {
                        propertyValidationResult.Description += " ({0})".FormatString(propertyValidationResult.IsValid ? "Valid" : "Invalid");
                    }

                    //is it ignored or not?
                    listy.Add(propertyValidationResult);
                }
            }

            return listy;
        }
    }

    public class DataMapValidationOptions
    {
        public Boolean ValidateKeys
        {
            get;
            set;
        }
        //public Boolean ValidateCollections
        //{
        //    get;
        //    private set;
        //}
        public DataMapValidationOptions()
        {

        }
        public DataMapValidationOptions(DataMapValidationOptions options)
        {
            this.ValidateKeys = options.ValidateKeys;
        }


        private static DataMapValidationOptions _default = null;
        public static DataMapValidationOptions Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new DataMapValidationOptions()
                    {
                        ValidateKeys = true
                        //ValidateCollections = true
                    };
                }

                return _default;
            }
        }

    }
}
