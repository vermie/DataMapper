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
            var sourcePropertyInfo = Utility.GetPropertyInfo<Source>(sourcePropertyExpression);

            this.IgnoreProperty(sourcePropertyInfo);

            return this;
        }

        public DataMapBuilder<Source, Target> MapProperty(Expression<Func<Source, object>> sourcePropertyExpression, Expression<Func<Target, object>> targetPropertyExpression)
        {
            return this.MapProperty(sourcePropertyExpression, targetPropertyExpression, MappedPropertyType.Field);
        }
        public DataMapBuilder<Source, Target> MapProperty(Expression<Func<Source, object>> sourcePropertyExpression, Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType)
        {
            return this.MapProperty(sourcePropertyExpression, targetPropertyExpression, mappedPropertyType, null);
        }
        public DataMapBuilder<Source, Target> MapProperty(Expression<Func<Source, object>> sourcePropertyExpression, Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType, ITypeConverter typeConverter)
        {
            var sourcePropertyInfo = Utility.GetPropertyInfo<Source>(sourcePropertyExpression);
            var targetPropertyInfo = Utility.GetPropertyInfo<Target>(targetPropertyExpression);

            this.MapProperty(sourcePropertyInfo, targetPropertyInfo, mappedPropertyType, typeConverter);

            return this;
        }
        public DataMapBuilder<Source, Target> MapPropertyByConvention(Expression<Func<Source, object>> sourcePropertyExpression)
        {
            var sourcePropertyInfo = Utility.GetPropertyInfo<Source>(sourcePropertyExpression);

            this.MapPropertyByConvention(sourcePropertyInfo);

            return this;
        }
    }

    

    
}
