using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DataMapper.Conversion;
using DataMapper.Mapping;

namespace DataMapper.Building
{

    public class DataMapBuilderCore
    {

        #region Private variables
        private DataMapBuilderCore _parent = null;
        private List<DataMapBuilderCore> _children = new List<DataMapBuilderCore>();

        internal DataMapBuilderState _state;
        protected DataMap _dataMap;
        private HashSet<PropertyInfo> _sourcePropertyInfoHashSet = new HashSet<PropertyInfo>();
        private HashSet<PropertyInfo> _targetPropertyInfoHashSet = new HashSet<PropertyInfo>();

        private HashSet<PropertyInfo> _sourceIgnoredPropertyInfoHashSet = new HashSet<PropertyInfo>();
        private HashSet<PropertyInfo> _targetIgnoredPropertyInfoHashSet = new HashSet<PropertyInfo>();

        #endregion

        #region Properties
        public DataMapValidationOptions ValidationOptions
        {
            get;
            set;
        }

        internal DataMapBuilderCore Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }
        internal List<DataMapBuilderCore> Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        internal DataMap DataMap
        {
            get
            {
                return this._dataMap;
            }
            set
            {
                this._dataMap = value;
            }
        }
        internal HashSet<PropertyInfo> SourcePropertyInfoHashSet
        {
            get
            {
                return this._sourcePropertyInfoHashSet;
            }
            set
            {
                this._sourcePropertyInfoHashSet = value;
            }
        }
        internal HashSet<PropertyInfo> TargetPropertyInfoHashSet
        {
            get
            {
                return _targetPropertyInfoHashSet;
            }
            set
            {
                this._targetPropertyInfoHashSet = value;
            }
        }
        internal HashSet<PropertyInfo> SourceIgnoredPropertyInfoHashSet
        {
            get
            {
                return _sourceIgnoredPropertyInfoHashSet;
            }
            set
            {
                this._sourceIgnoredPropertyInfoHashSet = value;
            }
        }
        internal HashSet<PropertyInfo> TargetIgnoredPropertyInfoHashSet
        {
            get
            {
                return _targetIgnoredPropertyInfoHashSet;
            }
            set
            {
                this._targetIgnoredPropertyInfoHashSet = value;
            }
        }

        internal Type SourceType
        {
            get
            {
                return this._dataMap.SourceType;
            }
        }
        internal Type TargetType
        {
            get
            {
                return this._dataMap.TargetType;
            }
        }     
        #endregion

        #region Constructors
        public DataMapBuilderCore(Type sourceType, Type targetType)
        {
            this._dataMap = new DataMap()
            {
                SourceType = sourceType,
                TargetType = targetType,
                Parent = null
            };
            this._state = new DataMapBuilderState();

            //get all the possible mapping properties
            _sourcePropertyInfoHashSet.AddRange(this.SourceType.GetMappingProperties());
            _targetPropertyInfoHashSet.AddRange(this.TargetType.GetMappingProperties());

            this.ValidationOptions = DataMapValidationOptions.Default;
        }
        internal DataMapBuilderCore(DataMap dataMap, DataMapBuilderState state)
        {
            this._dataMap = dataMap;
            this._state = state;

            _sourcePropertyInfoHashSet.AddRange(this.SourceType.GetMappingProperties());
            _targetPropertyInfoHashSet.AddRange(this.TargetType.GetMappingProperties());

            this.ValidationOptions = DataMapValidationOptions.Default;
        }
        internal DataMapBuilderCore(DataMapBuilderCore copy)
        {
            this.Parent = copy.Parent;
            this.Children = copy.Children;
            this.DataMap = copy.DataMap;

            this._state = copy._state;

            this.SourcePropertyInfoHashSet = copy.SourcePropertyInfoHashSet;
            this.SourceIgnoredPropertyInfoHashSet = copy.SourceIgnoredPropertyInfoHashSet;
            this.TargetPropertyInfoHashSet = copy.TargetPropertyInfoHashSet;
            this.TargetIgnoredPropertyInfoHashSet = copy.TargetIgnoredPropertyInfoHashSet;

            //ok as long as validationoptions class is immutable
            this.ValidationOptions = new DataMapValidationOptions(copy.ValidationOptions);
        }
        #endregion

        #region Validation

        //public DataMapValidation Validate()
        //{
        //    var validation = this.GetValidation();

        //    if (validation.IsEntireDataMapValid() == false)
        //    {
        //        throw new DataBuilderException(validation);
        //    }

        //    //return validation;
        //}
        //public DataMapValidation GetValidation()
        //{
        //    DataMapValidationBuilder validationBuilder = new DataMapValidationBuilder(this);

        //    return validationBuilder.Build();
        //}

        public DataMapValidation Validate()
        {
            DataMapValidationBuilder validationBuilder = new DataMapValidationBuilder(this);

            return validationBuilder.Build();
        }
        //public DataMapValidation GetValidation()
        //{
        //    DataMapValidationBuilder validationBuilder = new DataMapValidationBuilder(this);

        //    return validationBuilder.Build();
        //}

        #endregion

        #region Public methods

        public DataMapBuilderCore GetTopMapper()
        {
            return this.Top();
            //return new DataMapBuilderCore(this._dataMap.Top(), this._state);
        }
        public DataMapBuilderCore GetParentMapper()
        {
            return this.Parent;
            //return new DataMapBuilderCore(this._dataMap.Parent, this._state);
        }
        //public DataMapBuilderCore GetChildMapper(Type collectionSourceType, Type collectionTargetType)
        //{
        //    var result = this._dataMap.DataMapCollectionList.TryFindByItemDataMap(collectionSourceType, collectionTargetType);

        //    if (result == null)
        //    {
        //        throw new DataMapperException("Unable to retrieve child mapper");
        //    }

        //    var childBuilder = new DataMapBuilderCore(result.ItemDataMap, this._state);

        //    return childBuilder;
        //}

        public DataMapBuilderCore GetChildMapper(Type collectionSourceType, Type collectionTargetType)
        {
            //check to see if we have already created this one...if so, hand that one back
            var existingMapper = this.Children.Where(a => a.DataMap.TargetType == collectionTargetType && a.DataMap.SourceType == collectionSourceType).FirstOrDefault();
            if (existingMapper == null)
            {
                var result = this._dataMap.DataMapCollectionList.TryFindByItemDataMap(collectionSourceType, collectionTargetType);

                if (result == null)
                {
                    throw new DataMapperException("Unable to create child mapper '{0}'->'{1}'. Failed to find source collection with collection item type '{2}' mapped to target collection with collection item type '{3}'.".FormatString(
                        this._dataMap.SourceType.FullName,
                        this._dataMap.TargetType.FullName,
                        collectionSourceType.FullName,
                        collectionTargetType.FullName));
                }

                existingMapper = new DataMapBuilderCore(result.ItemDataMap, this._state);
                existingMapper.Parent = this;//set the parent

                this.Children.Add(existingMapper);
            }

            return existingMapper;
        }

        public DataMap FinalizeMap()
        {
            //if we have finalized, return the rootmap
            if (this._state.IsBuilderFinished)
            {
                return this._dataMap.Top();
            }

            //valid
            var topLevelMapper = this.GetTopMapper();

            var validation = topLevelMapper.Validate();

            if (validation.IsValid == false)
            {
                throw new DataBuilderException(validation);
            }

            var rootMap = topLevelMapper._dataMap;
            
            this._state.IsBuilderFinished = true;

            return rootMap;
        }

        public DataMapBuilderCore IgnoreProperty(PropertyInfo sourcePropertyInfo)
        {
            if(sourcePropertyInfo == null)
            {
                throw new ArgumentNullException("sourcePropertyInfo");
            }
            if (!this._sourcePropertyInfoHashSet.Contains(sourcePropertyInfo))
            {
                throw new DataMapperException("Cannot add property '{0}' to the ignore list because the property is not a member of '{1}'"
                    .FormatString(sourcePropertyInfo.Name,this.SourceType.FullName));
            }

            if (this._sourceIgnoredPropertyInfoHashSet.Contains(sourcePropertyInfo))
            {
                //should we do throw an exception here?
                throw new DataMapperException("Cannot add property '{0}' to the ignore list because the property is already marked as being ignored"
                    .FormatString(sourcePropertyInfo.Name));
            }
            else
            {
                this._sourceIgnoredPropertyInfoHashSet.Add(sourcePropertyInfo);
            }

            return this;
        }

        public DataMapBuilderCore MapProperty(PropertyInfo sourcePropertyInfo, PropertyInfo targetPropertyInfo, MappedPropertyType propertyType, ITypeConverter typeConverter)
        {
            var propertyMap = new PropertyMap(
                sourcePropertyInfo,
                targetPropertyInfo,
                propertyType == MappedPropertyType.KeyField,
                sourcePropertyInfo.IsPropertyTypeCollectionType());

            if (typeConverter != null)
            {
                //set this guy up
                propertyMap.TypeConverter = typeConverter;
            }

            //if we have a collection type here,
            //we need to do some more work.
            if (propertyMap.IsCollection)
            {
                //get the collection item types
                propertyMap.SourceCollectionItemType = propertyMap.SourcePropertyInfo.TryExtractPropertyTypeCollectionItemType();
                propertyMap.TargetCollectionItemType = propertyMap.TargetPropertyInfo.TryExtractPropertyTypeCollectionItemType();
            }

            //are they trying to map something that they put on the ignore list?
            //should we throw or let them do this? 
            if (this._sourceIgnoredPropertyInfoHashSet.Contains(sourcePropertyInfo))
            {
                //for now, let us just assumed if they mapped it that they are not ignoring it anymore
                this._sourceIgnoredPropertyInfoHashSet.Remove(sourcePropertyInfo);
            }

            //add the property map
            this.AddPropertyMap(propertyMap);

            return this;
        }

        public DataMapBuilderCore MapRemainingByConvention(PropertyMapUnresolvedBehavior behavior)
        {
            return this.MapRemainingByConvention(true, behavior == PropertyMapUnresolvedBehavior.None ? false : true);
        }
        //public DataMapBuilderCore TryMapRemainingByConvention()
        //{
        //    return this.MapRemainingByConvention(true,false);
        //}
        private DataMapBuilderCore MapRemainingByConvention(Boolean processAllChildrenRecursively, Boolean throwIfUnableToMap)
        {
            //map his stuff by convention.
            this.MapSourcePropertiesByConvention(throwIfUnableToMap);

            if (processAllChildrenRecursively)
            {
                //map the kids now
                foreach (var item in this._dataMap.DataMapCollectionList)
                {
                    //get a child mapper
                    var childMapper = this.GetChildMapper(item.ItemDataMap.SourceType, item.ItemDataMap.TargetType);

                    //do the remaining mapping
                    childMapper.MapRemainingByConvention(processAllChildrenRecursively,throwIfUnableToMap);
                }
            }
            return this;
        }

        //protected DataMapBuilderCore MapSourcePropertiesByConvention()
        //{
        //    return this.MapSourcePropertiesByConvention(true);
        //}
        //public DataMapBuilderCore TryMapSourcePropertiesByConvention()
        //{
        //    return this.MapSourcePropertiesByConvention(false);
        //}
        private DataMapBuilderCore MapSourcePropertiesByConvention(Boolean throwIfUnableToMap)
        {
            HashSet<PropertyInfo> mappedOrIgnoredProperties = new HashSet<PropertyInfo>();
            mappedOrIgnoredProperties.AddRange(this._dataMap.GetAllMappedProperties().Select(a => a.SourcePropertyInfo));
            mappedOrIgnoredProperties.AddRange(this._sourceIgnoredPropertyInfoHashSet);

            var fullOuterJoinResult =
                mappedOrIgnoredProperties.FullOuterJoin(
                this._sourcePropertyInfoHashSet,
                a => a,
                a => a,
                (x, y) => new Tuple<PropertyInfo, PropertyInfo>(x == null ? null : x, y));

            var remaining = fullOuterJoinResult.Where(a => a.Item1 == null).Select(a => a.Item2).ToList();

            remaining.ForEach(a => this.MapPropertyByConvention(a, throwIfUnableToMap));

            return this;
        }
        //private DataMapBuilderCore MapSourcePropertiesByConvention(Boolean throwIfUnableToMap)
        //{
        //    var fullOuterJoinResult =
        //        this._dataMap.GetAllMappedProperties().FullOuterJoin(
        //        this._sourcePropertyInfoHashSet,
        //        a => a.SourcePropertyInfo,
        //        a => a,
        //        (x, y) => new Tuple<PropertyInfo, PropertyInfo>(x == null ? null : x.SourcePropertyInfo, y));

        //    var remaining = fullOuterJoinResult.Where(a => a.Item1 == null).Select(a => a.Item2).ToList();

        //    remaining.ForEach(a => this.MapPropertyByConvention(a,throwIfUnableToMap));

        //    return this;
        //}

        public DataMapBuilderCore MapPropertyByConvention(PropertyInfo sourcePropertyInfo)
        {
            return this.MapPropertyByConvention(sourcePropertyInfo, true);
        }
        //public DataMapBuilderCore TryMapPropertyByConvention(PropertyInfo sourcePropertyInfo)
        //{
        //    return this.MapPropertyByConvention(sourcePropertyInfo, false);
        //}
        private DataMapBuilderCore MapPropertyByConvention(PropertyInfo sourcePropertyInfo,Boolean throwIfUnableToMap)
        {
            PropertyInfo targetPropertyInfo = null;

            if (sourcePropertyInfo == null)
            {
                throw new ArgumentNullException("sourcePropertyInfo");
            }

            //match by name and type for integrals, match by name only for collections.
            if (sourcePropertyInfo.IsPropertyTypeIntegralType())
            {
                targetPropertyInfo = this._targetPropertyInfoHashSet.Where(a => a.IsMappingMatch(
                    sourcePropertyInfo, MappingMatchOption.MatchByPropertyNameAndPropertyType)).FirstOrDefault();
            }
            else if (sourcePropertyInfo.IsPropertyTypeCollectionType())
            {//match for the collection type
                targetPropertyInfo = this._targetPropertyInfoHashSet.Where(a => a.IsMappingMatch(
                    sourcePropertyInfo, MappingMatchOption.MatchByPropertyName)).FirstOrDefault();
            }
            else
            {
                //If this is not an integral type or collection type, we will add it to the ignore list by convention.
                //allow these conventions to be changed later on? possibly
                return this.IgnoreProperty(sourcePropertyInfo);

                //throw new DataMapperException("Unable to map property by convention the type is not a collection type or an integral type (value type + string).");
            }


            if (targetPropertyInfo == null)
            {
                if (throwIfUnableToMap)
                {
                    throw new DataMapperException(
                        "Unable to find matching property for property '{0}' on type '{1}'. Please map this property manually, or make sure it can be mapped by the selected conventions."
                        .FormatString(sourcePropertyInfo.Name, sourcePropertyInfo.DeclaringType.FullName));
                }

                return this;
            }
            else
            {
                return this.MapProperty(sourcePropertyInfo, targetPropertyInfo,
                    this.IsKeyByNamingConvention(sourcePropertyInfo) || this.IsKeyByNamingConvention(targetPropertyInfo) ? MappedPropertyType.KeyField : MappedPropertyType.Field, null);
            }
        }

        #endregion

        #region Private/Protected methods

        private void AddPropertyMap(PropertyMap propertyMap)
        {
            if (this._state.IsBuilderFinished)
            {
                throw new DataMapperException("Cannot modify the current mapping schema because FinalizeMap() has been invoked. Create a new builder.");
            }

            //put this off for now
            //this.ValidatePropertyMap(propertyMap);

            if (propertyMap.IsCollection)
            {
                //add to our collection map here
                this._dataMap.DataMapCollectionList.Add(
                    new DataMapCollection(
                        new DataMap()
                        {
                            Parent = this._dataMap,
                            SourceType = propertyMap.SourceCollectionItemType,
                            TargetType = propertyMap.TargetCollectionItemType
                        },
                        propertyMap));
            }
            else
            {
                //add this property to the property map list. 
                this._dataMap.PropertyMapList.Add(propertyMap);
            }
        }
    
        private Boolean IsKeyByNamingConvention(PropertyMap propertyMap)
        {
            return this.IsKeyByNamingConvention(propertyMap.SourcePropertyInfo) || this.IsKeyByNamingConvention(propertyMap.TargetPropertyInfo);
        }

        private Boolean IsKeyByNamingConvention(PropertyInfo propertyInfo)
        {
            string keyName = String.Format("{0}Id", propertyInfo.DeclaringType.Name);

            return String.Compare(propertyInfo.Name, keyName, false) == 0;
        }

        #endregion

    }

    [Serializable()]
    public enum MappedPropertyType
    {
        KeyField,
        Field
    }

    [Serializable()]
    public enum PropertyMapUnresolvedBehavior
    {
        None,
        ThrowException
    }
    
}
