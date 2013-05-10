using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;

namespace DataMapper.TypeMapping
{
    public class TypeMapper<Source, Target> : DataMapBuilder<Source, Target>, ITypeMapper<Source, Target>
    {
        public TypeMapper()
        {
            this.ValidationOptions = new DataMapValidationOptions() { ValidateKeys = false };
        }

        public new ITypeMapper<Source, Target> IgnoreProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression)
        {
            base.IgnoreProperty(sourcePropertyExpression);

            return this;
        }

        public new ITypeMapper<Source, Target> MapProperty<TSourceProperty, TTargetProperty>(
            Expression<Func<Source, TSourceProperty>> sourcePropertyExpression,
            Expression<Func<Target, TTargetProperty>> targetPropertyExpression,
            MappedPropertyType mappedPropertyType = MappedPropertyType.Field,
            DataMapper.Conversion.ITypeConverter typeConverter = null)
        {
            base.MapProperty(sourcePropertyExpression, targetPropertyExpression, mappedPropertyType, typeConverter);

            return this;
        }

        public new ITypeMapper<Source, Target> MapPropertyByConvention(Expression<Func<Source, object>> sourcePropertyExpression)
        {
            base.MapPropertyByConvention(sourcePropertyExpression);

            return this;
        }
    }
}
