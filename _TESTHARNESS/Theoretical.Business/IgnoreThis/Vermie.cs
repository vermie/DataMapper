using System;

using System.Collections.Generic;

using System.Data.Entity.Infrastructure;

using System.Linq.Expressions;
using Theoretical.Data;
using System.Reflection;




namespace HoraceMann.APM.Repository
{

    public static class DbQueryExtensions
    {
        public static DbQuery<T> Include<T, U, V>(this DbQuery<T> source, Expression<Func<T, IEnumerable<U>>> exp, Expression<Func<U, IEnumerable<V>>> with, params Expression<Func<V, object>>[] plus)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            path = path + "." + GetPath(with);

            query = query.Include(path);




            foreach (var plusExp in plus)
            {

                query = query.Include(path + "." + GetPath(plusExp));

            }




            return query;

        }




        public static DbQuery<T> Include<T, U, V>(this DbQuery<T> source, Expression<Func<T, IEnumerable<U>>> exp, Expression<Func<U, V>> with, params Expression<Func<V, object>>[] plus)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            path = path + "." + GetPath(with);

            query = query.Include(path);




            foreach (var plusExp in plus)

                query = query.Include(path + "." + GetPath(plusExp));




            return query;

        }




        public static DbQuery<T> Include<T, U, V>(this DbQuery<T> source, Expression<Func<T, U>> exp, Expression<Func<U, IEnumerable<V>>> with, params Expression<Func<V, object>>[] plus)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            path = path + "." + GetPath(with);

            query = query.Include(path);




            foreach (var plusExp in plus)

                query = query.Include(path + "." + GetPath(plusExp));




            return query;

        }




        public static DbQuery<T> Include<T, U, V>(this DbQuery<T> source, Expression<Func<T, U>> exp, Expression<Func<U, V>> with, params Expression<Func<V, object>>[] plus)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            path = path + "." + GetPath(with);

            query = query.Include(path);




            foreach (var plusExp in plus)

                query = query.Include(path + "." + GetPath(plusExp));




            return query;

        }




        public static DbQuery<T> Include<T, U>(this DbQuery<T> source, Expression<Func<T, IEnumerable<U>>> exp, params Expression<Func<U, object>>[] with)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            foreach (var withExp in with)

                query = query.Include(path + "." + GetPath(withExp));




            return query;

        }




        public static DbQuery<T> Include<T, U>(this DbQuery<T> source, Expression<Func<T, U>> exp, params Expression<Func<U, object>>[] with)
        {

            var path = GetPath(exp);

            var query = source.Include(path);




            foreach (var withExp in with)

                query = query.Include(path + "." + GetPath(withExp));




            return query;

        }




        public static DbQuery<T> Include<T>(this DbQuery<T> source, params Expression<Func<T, object>>[] exps)
        {

            var query = source;




            foreach (var exp in exps)
            {

                var path = GetPath(exp);

                query = source.Include(path);

            }




            return query;

        }




        private static string GetPath(Expression exp)
        {

            switch (exp.NodeType)
            {

                case ExpressionType.MemberAccess:

                    var name = GetPath(((MemberExpression)exp).Expression) ?? "";




                    if (name.Length > 0)

                        name += ".";




                    return name + ((MemberExpression)exp).Member.Name;




                case ExpressionType.Convert:

                case ExpressionType.Quote:

                    return GetPath(((UnaryExpression)exp).Operand);




                case ExpressionType.Lambda:

                    return GetPath(((LambdaExpression)exp).Body);




                default:

                    return null;

            }

        }


        public static MemberExpression GetTerminalMemberExpression<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            ParameterExpression parameter;
            Expression instance;
            MemberExpression propertyOrField;

            GetMemberExpression(expression, out parameter, out instance, out propertyOrField);

            if (propertyOrField == null || !(propertyOrField.Member is PropertyInfo || propertyOrField.Member is FieldInfo))
                throw new ArgumentException("must return a property or field", "expression");

            return propertyOrField;
        }

        private static void GetMemberExpression<T, U>(Expression<Func<T, U>> expression, out ParameterExpression parameter, out Expression instance, out MemberExpression propertyOrField)
        {
            Expression current = expression.Body;

            while (current.NodeType == ExpressionType.Convert || current.NodeType == ExpressionType.TypeAs)
            {
                current = (current as UnaryExpression).Operand;
            }

            if (current.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException();
            }

            propertyOrField = current as MemberExpression;
            current = propertyOrField.Expression;

            instance = current;

            while (current.NodeType != ExpressionType.Parameter)
            {
                if (current.NodeType == ExpressionType.Convert || current.NodeType == ExpressionType.TypeAs)
                {
                    current = (current as UnaryExpression).Operand;
                }
                else if (current.NodeType == ExpressionType.MemberAccess)
                {
                    current = (current as MemberExpression).Expression;
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            parameter = current as ParameterExpression;
        }


    }
}
