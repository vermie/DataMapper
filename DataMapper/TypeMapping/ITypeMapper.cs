using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;

namespace DataMapper.TypeMapping
{
    public interface ITypeMapper<Source, Target>
    {
        ITypeMapper<Source, Target> IgnoreProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression);
        ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression);
        ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType);
        ITypeMapper<Source, Target> MapProperty(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression, System.Linq.Expressions.Expression<Func<Target, object>> targetPropertyExpression, MappedPropertyType mappedPropertyType, DataMapper.Conversion.ITypeConverter typeConverter);
        ITypeMapper<Source, Target> MapPropertyByConvention(System.Linq.Expressions.Expression<Func<Source, object>> sourcePropertyExpression);

        DataMapper.Mapping.DataMap FinalizeMap();
        DataMapBuilderCore MapRemainingByConvention(PropertyMapUnresolvedBehavior behavior);
        DataMapValidation Validate();
    }
}
