using System;
using System.Collections.Generic;
using System.Linq;
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

        public new ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression)
        {
            base.MapProperty(sourcePropertyExpression, targetPropertyExpression);

            return this;
        }

        public new ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType)
        {
            base.MapProperty(sourcePropertyExpression, targetPropertyExpression, mappedPropertyType);

            return this;
        }

        public new ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType, Conversion.ITypeConverter typeConverter)
        {
            base.MapProperty(sourcePropertyExpression, targetPropertyExpression, mappedPropertyType, typeConverter);

            return this;
        }

        public new ITypeMapper<Source, Target> MapPropertyByConvention(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression)
        {
            base.MapPropertyByConvention(sourcePropertyExpression);

            return this;
        }
    }
}
