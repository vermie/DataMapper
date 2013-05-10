using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using DataMapper.Conversion;

namespace DataMapper.Building
{


    public class DataMapBuilder<Source, Target> : DataMapBuilderCore
    {
        public DataMapBuilder()
            : base(typeof(Source), typeof(Target))
        {

        }
        internal DataMapBuilder(DataMapBuilderCore copy)
            : base(copy)
        {

        }

        public DataMapBuilder<CollectionSource, CollectionTarget> GetChildMapper<CollectionSource, CollectionTarget>()
        {
            var childMapper = this.GetChildMapper(typeof(CollectionSource), typeof(CollectionTarget));

            return new DataMapBuilder<CollectionSource, CollectionTarget>(childMapper);
        }

        public DataMapBuilder<Source, Target> IgnoreProperty(Expression<Func<Source, object>> sourcePropertyExpression)
        {
            var sourcePropertyInfo = Utility.GetPropertyInfo(sourcePropertyExpression);

            this.IgnoreProperty(sourcePropertyInfo);

            return this;
        }

        public DataMapBuilder<Source, Target> MapProperty<TSourceProperty, TTargetProperty>(
            Expression<Func<Source, TSourceProperty>> sourcePropertyExpression,
            Expression<Func<Target, TTargetProperty>> targetPropertyExpression,
            MappedPropertyType mappedPropertyType = MappedPropertyType.Field,
            ITypeConverter typeConverter = null)
        {
            var sourcePropertyInfo = Utility.GetPropertyInfo(sourcePropertyExpression);
            var targetPropertyInfo = Utility.GetPropertyInfo(targetPropertyExpression);

            this.MapProperty(sourcePropertyInfo, targetPropertyInfo, mappedPropertyType, typeConverter);

            return this;
        }
        public DataMapBuilder<Source, Target> MapPropertyByConvention(Expression<Func<Source, object>> sourcePropertyExpression)
        {
            var sourcePropertyInfo = Utility.GetPropertyInfo(sourcePropertyExpression);

            this.MapPropertyByConvention(sourcePropertyInfo);

            return this;
        }
    }

    

    
}
