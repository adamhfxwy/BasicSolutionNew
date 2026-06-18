
namespace Solution.Core.Infrastructure.Utils
{
    public class CommonWhereBuilder  //<TEntity, TQuery> where TEntity : class
    {
        public static async Task<Tuple<IQueryable<TEntity>, int>> WhereBuilder<TEntity, TQuery>(IQueryable<TEntity> data, TQuery query, CancellationToken cancellationToken = default
            , bool isPage = true, bool isOrderby = true) where TEntity : class
        {
            var types = query.GetType();
            var entityType = typeof(TEntity);
            var result = data.AsNoTracking();

            foreach (PropertyInfo queryProperty in types.GetProperties())
            {
                ContainsPropAttribute containsAttribute = (ContainsPropAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(ContainsPropAttribute));
                SearchPropertyAttribute propertyAttribute = (SearchPropertyAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchPropertyAttribute));
                SearchDateStartAttribute dateStartAttribute = (SearchDateStartAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateStartAttribute));
                SearchDateEndAttribute dateEndAttribute = (SearchDateEndAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateEndAttribute));
                SearchDateEndLessThanOrEqualAttribute dateEnddLessThanOrEqualAttribute = (SearchDateEndLessThanOrEqualAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateEndLessThanOrEqualAttribute));
                NavigationAttribute navigationAttribute = (NavigationAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(NavigationAttribute));
                CascadeNavigationAttribute cascadeNavigationAttribute = (CascadeNavigationAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(CascadeNavigationAttribute));
                if (propertyAttribute == null)
                {
                    continue;
                }
                foreach (PropertyInfo entityProperty in entityType.GetProperties())
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "entity");
                    var property = Expression.Property(parameter, entityProperty);

                    if (entityProperty.Name == propertyAttribute.PropertyName && entityProperty.Name != "Error" && entityProperty.Name != "Item" && navigationAttribute == null && cascadeNavigationAttribute == null) // 判断属性名是否相同  
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {


                            if (containsAttribute != null)//模糊匹配
                            {
                                var constant = Expression.Constant(queryValue, entityProperty.PropertyType);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(property, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(property, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end, entityProperty.PropertyType);
                                var equalExpression = Expression.LessThan(property, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateEnddLessThanOrEqualAttribute != null)// 小于等于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end, entityProperty.PropertyType);
                                var equalExpression = Expression.LessThanOrEqual(property, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else //一般的对比 （==）
                            {
                                var constant = Expression.Constant(queryValue, entityProperty.PropertyType);
                                var equalExpression = Expression.Equal(property, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }

                        }
                        break;
                    }
                    else if (navigationAttribute != null)
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {
                            // 通过反射获取导航属性的类型
                            var navigationProperty = typeof(TEntity).GetProperty(navigationAttribute.EntityName);
                            if (navigationProperty == null)
                            {
                                continue;
                            }
                            var navigationPropertyValue = Expression.Property(parameter, navigationProperty);
                            var navigationPropertyType = navigationPropertyValue.Type;

                            // 获取导航属性的指定属性
                            var propertyNavigation = navigationPropertyType.GetProperty(propertyAttribute.PropertyName);
                            if (propertyNavigation == null)
                            {
                                continue;
                            }

                            var propertyAccess = Expression.Property(navigationPropertyValue, propertyNavigation);

                            // var equal = Expression.Equal(propertyAccess, constant);
                            if (containsAttribute != null)//模糊匹配
                            {
                                var constant = Expression.Constant(queryValue);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(propertyAccess, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end);
                                var equalExpression = Expression.LessThan(propertyAccess, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else //一般的对比 （==）
                            {
                                var constant = Expression.Constant(queryValue);
                                var equalExpression = Expression.Equal(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            // var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
                            // 应用 Where 条件并返回结果
                            // result = result.Where(lambda);
                        }
                        break;
                    }
                    else if (cascadeNavigationAttribute != null)
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {
                            var navigationProperties = cascadeNavigationAttribute.EntityName.Split('.');
                            Expression propertyExpression = parameter;
                            foreach (var navigationProperty in navigationProperties)
                            {
                                var propertyCascadeNavigation = propertyExpression.Type.GetProperty(navigationProperty);
                                if (propertyCascadeNavigation == null)
                                {
                                    continue;
                                }
                                propertyExpression = Expression.Property(propertyExpression, propertyCascadeNavigation);
                            }
                            // 获取最终的属性
                            var finalProperty = propertyExpression.Type.GetProperty(propertyAttribute.PropertyName);
                            if (finalProperty == null)
                            {
                                continue;
                            }

                            var propertyAccess = Expression.Property(propertyExpression, finalProperty);

                            if (containsAttribute != null)//模糊匹配
                            {
                                var constant = Expression.Constant(queryValue);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(propertyAccess, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end);
                                var equalExpression = Expression.LessThan(propertyAccess, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                            else //一般的对比 （==）
                            {
                                var constant = Expression.Constant(queryValue);
                                var equalExpression = Expression.Equal(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                result = result.Where(lambda);
                            }
                        }
                        break;
                    }
                }
            }
            // 获取数据条数
            int count = await result.CountAsync(cancellationToken); // 异步查询数据条数           
            //排序
            if (isOrderby)
            {
                var orderProperty = (string)query.GetType().GetProperty("OrderBy").GetValue(query);
                if (!string.IsNullOrEmpty(orderProperty))
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "entity");
                    var property = Expression.Property(parameter, orderProperty);
                    var orderLambda = Expression.Lambda<Func<TEntity, dynamic>>(Expression.Convert(property, typeof(object)), parameter);

                    bool isDescending = (bool?)query.GetType().GetProperty("IsDescending").GetValue(query) ?? false;

                    if (isDescending)
                    {
                        result = result.OrderByDescending(orderLambda);
                    }
                    else
                    {
                        result = result.OrderBy(orderLambda);
                    }
                }
            }
            //分页
            if (isPage)
            {
                int? pageIndex = (int?)query.GetType().GetProperty("PageIndex").GetValue(query);
                int? PageSize = (int?)query.GetType().GetProperty("PageSize").GetValue(query);
                if (pageIndex.HasValue && PageSize.HasValue)
                {
                    result = result.Skip(pageIndex.Value * PageSize.Value).Take(PageSize.Value);
                }
            }
            return new Tuple<IQueryable<TEntity>, int>(result, count);
        }
        public static async Task<Expression<Func<TEntity, bool>>> WhereBuilderToExp<TEntity, TQuery>(TQuery query, CancellationToken cancellationToken = default
          , params string[] propertyNames) where TEntity : class
        {
            Expression<Func<TEntity, bool>> exp = i => true;
            var types = query.GetType();
            var entityType = typeof(TEntity);

            foreach (PropertyInfo queryProperty in types.GetProperties())
            {
                ContainsPropAttribute containsAttribute = (ContainsPropAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(ContainsPropAttribute));
                SearchPropertyAttribute propertyAttribute = (SearchPropertyAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchPropertyAttribute));
                SearchDateStartAttribute dateStartAttribute = (SearchDateStartAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateStartAttribute));
                SearchDateEndAttribute dateEndAttribute = (SearchDateEndAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateEndAttribute));
                SearchDateEndLessThanOrEqualAttribute dateEnddLessThanOrEqualAttribute = (SearchDateEndLessThanOrEqualAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(SearchDateEndLessThanOrEqualAttribute));
                NavigationAttribute navigationAttribute = (NavigationAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(NavigationAttribute));
                CascadeNavigationAttribute cascadeNavigationAttribute = (CascadeNavigationAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(CascadeNavigationAttribute));
                //SearchPropertyNotEqualAttribute searchPropertyNotEqualAttribute = (SearchPropertyNotEqualAttribute)Attribute.GetCustomAttribute(queryProperty, typeof(CascadeNavigationAttribute));
                if (propertyAttribute == null)
                {
                    continue;
                }
                foreach (PropertyInfo entityProperty in entityType.GetProperties())
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "entity");
                    var property = Expression.Property(parameter, entityProperty);
                    if (entityProperty.Name == propertyAttribute.PropertyName && entityProperty.Name != "Error" && entityProperty.Name != "Item" && navigationAttribute == null && cascadeNavigationAttribute == null) // 判断属性名是否相同  
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {


                            if (containsAttribute != null)//模糊匹配
                            {

                                var constant = Expression.Constant(queryValue, entityProperty.PropertyType);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(property, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(property, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end, entityProperty.PropertyType);
                                var equalExpression = Expression.LessThan(property, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateEnddLessThanOrEqualAttribute != null)// 小于等于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end, entityProperty.PropertyType);
                                var equalExpression = Expression.LessThanOrEqual(property, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else //一般的对比 （==）
                            {

                                var constant = Expression.Constant(queryValue, entityProperty.PropertyType);
                                var equalExpression = Expression.Equal(property, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }

                        }
                        break;
                    }
                    else if (navigationAttribute != null)
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {
                            // 通过反射获取导航属性的类型
                            var navigationProperty = typeof(TEntity).GetProperty(navigationAttribute.EntityName);
                            if (navigationProperty == null)
                            {
                                continue;
                            }
                            var navigationPropertyValue = Expression.Property(parameter, navigationProperty);
                            var navigationPropertyType = navigationPropertyValue.Type;

                            // 获取导航属性的指定属性
                            var propertyNavigation = navigationPropertyType.GetProperty(propertyAttribute.PropertyName);
                            if (propertyNavigation == null)
                            {
                                continue;
                            }

                            var propertyAccess = Expression.Property(navigationPropertyValue, propertyNavigation);

                            // var equal = Expression.Equal(propertyAccess, constant);
                            if (containsAttribute != null)//模糊匹配
                            {
                                var constant = Expression.Constant(queryValue);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(propertyAccess, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end);
                                var equalExpression = Expression.LessThan(propertyAccess, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else //一般的对比 （==）
                            {
                                var constant = Expression.Constant(queryValue);
                                var equalExpression = Expression.Equal(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            // var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
                            // 应用 Where 条件并返回结果
                            // result = result.Where(lambda);
                        }
                        break;
                    }
                    else if (cascadeNavigationAttribute != null)
                    {
                        var queryValue = queryProperty.GetValue(query);

                        if (queryValue != null && queryValue != "")
                        {
                            var navigationProperties = cascadeNavigationAttribute.EntityName.Split('.');
                            Expression propertyExpression = parameter;
                            foreach (var navigationProperty in navigationProperties)
                            {
                                var propertyCascadeNavigation = propertyExpression.Type.GetProperty(navigationProperty);
                                if (propertyCascadeNavigation == null)
                                {
                                    continue;
                                }
                                propertyExpression = Expression.Property(propertyExpression, propertyCascadeNavigation);
                            }
                            // 获取最终的属性
                            var finalProperty = propertyExpression.Type.GetProperty(propertyAttribute.PropertyName);
                            if (finalProperty == null)
                            {
                                continue;
                            }

                            var propertyAccess = Expression.Property(propertyExpression, finalProperty);

                            if (containsAttribute != null)//模糊匹配
                            {
                                var constant = Expression.Constant(queryValue);
                                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var equalExpression = Expression.Call(propertyAccess, method, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateStartAttribute != null)// 大于等于的比较
                            {
                                var start = Convert.ToDateTime(queryValue);
                                var constant = Expression.Constant(start, entityProperty.PropertyType);
                                var equalExpression = Expression.GreaterThanOrEqual(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else if (dateEndAttribute != null)// 小于的比较
                            {
                                var end = Convert.ToDateTime(queryValue).AddDays(1);
                                var constantEnd = Expression.Constant(end);
                                var equalExpression = Expression.LessThan(propertyAccess, constantEnd);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                            else //一般的对比 （==）
                            {
                                var constant = Expression.Constant(queryValue);
                                var equalExpression = Expression.Equal(propertyAccess, constant);
                                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                                exp = exp.And(lambda);
                            }
                        }
                        break;
                    }
                }
            }
            return exp;
        }
    }
}
