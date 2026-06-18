
namespace Solution.Core.Infrastructure.Utils
{
    public static class ExpressionExt
    { 
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var sum = Expression.And(expr1.Body, Expression.Invoke(expr2, expr1.Parameters[0]));
            return Expression.Lambda<Func<T, bool>>(sum, expr1.Parameters);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var sum = Expression.Or(expr1.Body, Expression.Invoke(expr2, expr1.Parameters[0]));
            return Expression.Lambda<Func<T, bool>>(sum, expr1.Parameters);
        }

        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> firstExpr, bool condition, Expression<Func<T, bool>> secondExpr)
    where T : class
        {
            return condition ? And(firstExpr, secondExpr) : firstExpr;
        }

        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>>? expr = null)
            where T : class
            => expr ?? (x => true);

        public static Expression<Func<T1, T2, bool>> New<T1, T2>(Expression<Func<T1, T2, bool>>? expr = null)
            where T1 : class
            where T2 : class
            => expr ?? ((x, y) => true);

    }
}
