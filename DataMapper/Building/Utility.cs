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

        internal static PropertyInfo GetPropertyInfo<TObject>(Expression<Func<TObject, object>> propertyRefExpr)
        {
            String propertyName = GetPropertyNameCore(propertyRefExpr.Body);

            var pInfo = typeof(TObject).GetProperty(propertyName);

            if (pInfo == null)
                throw new ArgumentException("Property '{0}' not found on type '{0}'".FormatString(propertyName, typeof(TObject).FullName));

            return pInfo;
        }

        private static string GetPropertyNameCore(Expression propertyRefExpr)
        {
            if (propertyRefExpr == null)
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");

            MemberExpression memberExpr = propertyRefExpr as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyRefExpr as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                return memberExpr.Member.Name;

            throw new ArgumentException("No property reference expression was found.",
                             "propertyRefExpr");
        }
        
    }
}
