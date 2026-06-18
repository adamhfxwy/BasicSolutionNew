using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Infrastructure.Utils
{
    public class SelectUtil
    {
        public static async Task<IEnumerable<object[]>> GetSelectCol<T>(IQueryable<T> ctx, CancellationToken cancellationToken = default, params string[] propertyNames) where T : class
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            List<Expression> exProps = new List<Expression>();
            foreach (string propName in propertyNames)
            {
                Expression exProp = Expression.Convert(Expression.MakeMemberAccess(
                    param, typeof(T).GetProperty(propName)), typeof(object));
                exProps.Add(exProp);
            }
            Expression[] initializers = exProps.ToArray();
            NewArrayExpression newArrayExp = Expression.NewArrayInit(typeof(object), initializers);
            var selectExpression = Expression.Lambda<Func<T, object[]>>(newArrayExp, param);

            IEnumerable<object[]> selectQueryable = await ctx.Select(selectExpression).ToListAsync(cancellationToken);
            return selectQueryable;
        }
        public static Expression<Func<TEntity, object[]>> GetSelectCol<TEntity>(params string[] propertyNames) where TEntity : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            List<Expression> expressions = new List<Expression>();
            foreach (string propName in propertyNames)
            {
                expressions.Add(Expression.Property(param, propName));
            }
            NewArrayExpression newArrayExp = Expression.NewArrayInit(typeof(object), expressions.Select(m => Expression.Convert(m, typeof(object))));
            var selectExpression = Expression.Lambda<Func<TEntity, object[]>>(newArrayExp, param);
            return selectExpression;
        }

        public static T ToDtoByObject<T>(object[] res, T t, params string[] param) where T : class
        {
            List<object> propList = new List<object>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type type = t.GetType();
            for (int i = 0; i < param.Length; i++)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.Name == param[i])
                    {
                        prop.SetValue(t, res[i]);
                        dic.Add(param[i], res[i]);
                        break;
                    }
                }
            }
            return t;
        }
        public static IEnumerable<TEntity> GetSelectedEntities<TEntity>(List<object[]> results, params string[] propertyNames) where TEntity : class
        {

            var entities = new List<TEntity>();
            foreach (var result in results)
            {
                var entity = Activator.CreateInstance<TEntity>(); // 创建 TEntity 的实例

                var entityType = typeof(TEntity);
                var properties = entityType.GetProperties();

                for (int i = 0; i < propertyNames.Length; i++)
                {
                    var propertyName = propertyNames[i];
                    var property = properties.FirstOrDefault(p => p.Name == propertyName);
                    if (property != null && i < result.Length)
                    {
                        var value = result[i];
                        if (value != null)
                        {
                            // 将值转换为属性的类型并设置属性值
                            var convertedValue = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(entity, convertedValue);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }
    }
}
