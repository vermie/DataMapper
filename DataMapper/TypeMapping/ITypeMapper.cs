using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;

namespace DataMapper.TypeMapping
{
    public interface ITypeMapper<Source, Target>
    {
        ITypeMapper<Source, Target> IgnoreProperty(Expression<Func<Source, object>> sourcePropertyExpression);

        ITypeMapper<Source, Target> MapProperty<TSourceProperty, TTargetProperty>(
            Expression<Func<Source, TSourceProperty>> sourcePropertyExpression,
            Expression<Func<Target, TTargetProperty>> targetPropertyExpression,
            MappedPropertyType mappedPropertyType = MappedPropertyType.Field,
            DataMapper.Conversion.ITypeConverter typeConverter = null);

        ITypeMapper<Source, Target> MapPropertyByConvention(Expression<Func<Source, object>> sourcePropertyExpression);

        DataMapper.Mapping.DataMap FinalizeMap();
        DataMapBuilderCore MapRemainingByConvention(PropertyMapUnresolvedBehavior behavior);
        DataMapValidation Validate();
    }
}
