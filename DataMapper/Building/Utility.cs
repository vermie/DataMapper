using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace DataMapper.Building
{
    internal class Utility
    {

        internal static PropertyInfo GetPropertyInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyRefExpr)
        {
            var body = propertyRefExpr.Body;
            var expr = propertyRefExpr.Body as MemberExpression;

            // includes things like:
            //   casts
            //   implicit/explicit conversion operators
            //   VB's CType
            //   boxed value types
            // probably should not support these, instead strictly enforce member access
            while (expr == null && body.NodeType == ExpressionType.Convert)
            {
                var convert = (UnaryExpression)body;
                expr = convert.Operand as MemberExpression;
                body = expr;
            }

            if (expr == null || !(expr.Member is PropertyInfo))
                throw new ArgumentException("expression '{0}' must be a property-access expression".FormatString(propertyRefExpr), "expression");

            return (PropertyInfo)expr.Member;
        }

    }
}
