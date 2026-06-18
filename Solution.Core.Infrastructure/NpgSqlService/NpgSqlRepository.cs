
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class NpgSqlRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly NpgSqlContext  _npgSqlContext; 
        public NpgSqlRepository(NpgSqlContext npgSqlContext)
        {
            _npgSqlContext = npgSqlContext;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = (await _npgSqlContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }

            return savedEntity;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entityArray = entities.ToArray();

            await _npgSqlContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _npgSqlContext.Attach(entity);

            var updatedEntity = _npgSqlContext.Update(entity).Entity;

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _npgSqlContext.Set<TEntity>().UpdateRange(entities);

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// 根据实体真删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _npgSqlContext.Set<TEntity>().Remove(entity);

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// 根据条件真删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbSet = _npgSqlContext.Set<TEntity>();

            var entities = await dbSet
                .Where(predicate)
                .ToListAsync(cancellationToken);

            await DeleteManyAsync(entities, autoSave, cancellationToken);

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// 批量真删除
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _npgSqlContext.RemoveRange(entities);

            if (autoSave)
            {
                await _npgSqlContext.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity>? query = _npgSqlContext.Set<TEntity>();
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.FirstOrDefaultAsync(cancellationToken);

        }
        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(long id ,Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity>? query = _npgSqlContext.Set<TEntity>();
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
            //return await _npgSqlContext.Set<TEntity>().FindAsync(id);
        }
        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <param name="searchBase"></param>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        public async Task<Pagination<TEntity>> GetListOfSignalTableAsync(
           QueryBase searchBase,
           Expression<Func<TEntity, bool>>? predicate = null,
           Expression<Func<TEntity, object[]>>? selectExpression = null,
           CancellationToken cancellationToken = default,
           bool disableTracking = true,
           params string[] param)
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (!string.IsNullOrEmpty(searchBase.OrderBy))
            {
                var orderType = searchBase.IsDescending ? "DESC" : "ASC";
                string sort = $"{searchBase.OrderBy} {orderType}";
                query = query.OrderBy(sort);
            }

            if (searchBase.PageIndex.HasValue && searchBase.PageSize.HasValue)
            {
                var pageResult = query.PageResult(searchBase.PageIndex.Value + 1, searchBase.PageSize.Value);
                if (selectExpression != null && param.Length > 0)
                {
                    var data = await pageResult.Queryable.Select(selectExpression).ToListAsync(cancellationToken);//
                    var list = GetSelectedEntities<TEntity>(data, cancellationToken, param);
                    var result = new Pagination<TEntity>()
                    {
                        List = list.ToList(),
                        Total = pageResult.RowCount
                    };
                    return await Task.FromResult(result);
                }
                else
                {
                    var data = await pageResult.Queryable.ToListAsync(cancellationToken);//.Select(selectExpression)
                                                                                         //var list =await GetSelectedEntities<TEntity>(data, cancellationToken,param);
                    var result = new Pagination<TEntity>()
                    {
                        List = data,// list.ToList(),
                        Total = pageResult.RowCount
                    };
                    return await Task.FromResult(result);
                }
            }
            else
            {
                if (selectExpression != null && param.Length > 0)
                {
                    var data = await query.Select(selectExpression).ToListAsync(cancellationToken);
                    var list = GetSelectedEntities<TEntity>(data, cancellationToken, param);
                    var result = new Pagination<TEntity>()
                    {
                        List = list.ToList(),
                        Total = list.Count()
                    };
                    return await Task.FromResult(result);
                }
                else
                {
                    var list = await query.ToListAsync(cancellationToken);
                    var result = new Pagination<TEntity>()
                    {
                        List = list,
                        Total = list.Count()
                    };
                    return await Task.FromResult(result);
                }

            }


        }
        public async Task<Pagination<TEntity>> GetListOfWithIncludDataAsync(QueryBase searchBase, Expression<Func<TEntity, bool>>? predicate = null
            , Expression<Func<TEntity, object[]>>? selectExpression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
            , CancellationToken cancellationToken = default, bool disableTracking = true, params string[] param)
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (!string.IsNullOrEmpty(searchBase.OrderBy))
            {
                var orderType = searchBase.IsDescending ? "DESC" : "ASC";
                string sort = $"{searchBase.OrderBy} {orderType}";
                query = query.OrderBy(sort);
            }

            if (searchBase.PageIndex.HasValue && searchBase.PageSize.HasValue)
            {
                var pageResult = query.PageResult(searchBase.PageIndex.Value + 1, searchBase.PageSize.Value);
                if (selectExpression != null && param.Length > 0)
                {
                    var data = await pageResult.Queryable.Select(selectExpression).ToListAsync(cancellationToken);//
                    var list = GetSelectedEntities<TEntity>(data, cancellationToken, param);
                    var result = new Pagination<TEntity>()
                    {
                        List = list.ToList(),
                        Total = pageResult.RowCount
                    };
                    return await Task.FromResult(result);
                }
                else
                {
                    var data = await pageResult.Queryable.ToListAsync(cancellationToken);//.Select(selectExpression)
                                                                                         //var list =await GetSelectedEntities<TEntity>(data, cancellationToken,param);
                    var result = new Pagination<TEntity>()
                    {
                        List = data,// list.ToList(),
                        Total = pageResult.RowCount
                    };
                    return await Task.FromResult(result);
                }
            }
            else
            {
                if (selectExpression != null && param.Length > 0)
                {
                    var data = await query.Select(selectExpression).ToListAsync(cancellationToken);
                    var list = GetSelectedEntities<TEntity>(data, cancellationToken, param);
                    var result = new Pagination<TEntity>()
                    {
                        List = list.ToList(),
                        Total = list.Count()
                    };
                    return await Task.FromResult(result);
                }
                else
                {
                    var list = await query.ToListAsync(cancellationToken);
                    var result = new Pagination<TEntity>()
                    {
                        List = list,
                        Total = list.Count()
                    };
                    return await Task.FromResult(result);
                }

            }
        }
        public async Task<Pagination<TEntity>> GetListOfWithIncludDataAsync(
            QueryBase searchBase
            , Expression<Func<TEntity, bool>>? predicate = null
            , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
            , CancellationToken cancellationToken = default
            , bool disableTracking = true
            )
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (!string.IsNullOrEmpty(searchBase.OrderBy))
            {
                var orderType = searchBase.IsDescending ? "DESC" : "ASC";
                string sort = $"{searchBase.OrderBy} {orderType}";
                query = query.OrderBy(sort);
            }

            if (searchBase.PageIndex.HasValue && searchBase.PageSize.HasValue)
            {
                var pageResult = query.PageResult(searchBase.PageIndex.Value + 1, searchBase.PageSize.Value);
                var data = await pageResult.Queryable.ToListAsync(cancellationToken);//.Select(selectExpression)
                                                                                     //var list =await GetSelectedEntities<TEntity>(data, cancellationToken,param);
                var result = new Pagination<TEntity>()
                {
                    List = data,// list.ToList(),
                    Total = pageResult.RowCount
                };
                return await Task.FromResult(result);
            }
            else
            {
                var list = await query.ToListAsync(cancellationToken);
                var result = new Pagination<TEntity>()
                {
                    List = list,
                    Total = list.Count()
                };
                return await Task.FromResult(result);
            }
        }
        private static IEnumerable<TEntity> GetSelectedEntities<TEntity>(List<object[]> results, CancellationToken cancellationToken = default, params string[] propertyNames) where TEntity : class
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

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _npgSqlContext.Set<TEntity>().CountAsync(predicate,cancellationToken);
        }
        /// <summary>
        /// 获取全部数据的IQueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllAsync()
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();
            return query;
        }
        /// <summary>
        /// 根据条件查看表中是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _npgSqlContext.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            await _npgSqlContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        public async Task<Pagination<TDto>> GetListOfWithIncludSelectDataAsync<TDto>(QueryBase searchBase,
            Expression<Func<TEntity, TDto>> selectExpression,
           Expression<Func<TEntity, bool>>? predicate = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
           , CancellationToken cancellationToken = default, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(searchBase.OrderBy))
            {
                var orderType = searchBase.IsDescending ? "DESC" : "ASC";
                query = query.OrderBy($"{searchBase.OrderBy} {orderType}");
            }

            // 分页
            if (searchBase.PageIndex.HasValue && searchBase.PageSize.HasValue)
            {
                var pageResult = query.PageResult(
                    searchBase.PageIndex.Value + 1,
                    searchBase.PageSize.Value
                );

                var list = await pageResult.Queryable
                    .Select(selectExpression)
                    .ToListAsync(cancellationToken);

                return new Pagination<TDto>
                {
                    List = list,
                    Total = pageResult.RowCount
                };
            }

            // 不分页
            var data = await query
                .Select(selectExpression)
                .ToListAsync(cancellationToken);

            return new Pagination<TDto>
            {
                List = data,
                Total = data.Count
            };
        }

        public async Task<TDto?> FindDtoByIdAsync<TDto>(long id, Expression<Func<TEntity, TDto>> selector, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
         CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();

            if (include != null)
                query = include(query);

            // ⚠️ 注意：Where 一定在 Entity 上
            query = query.Where(e => EF.Property<long>(e, "Id") == id);

            // ✅ 最后一步才 Select DTO
            return await query.Select(selector)
                              .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<TDto?> FindDtoAsync<TDto>(
              Expression<Func<TEntity, bool>> predicate,
              Expression<Func<TEntity, TDto>> selector,
              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
              CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _npgSqlContext.Set<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);
            if (include != null)
            {
                query = include(query);
            }
            return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
