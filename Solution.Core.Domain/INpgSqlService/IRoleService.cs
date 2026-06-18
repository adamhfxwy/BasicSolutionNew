

using Solution.Core.DTO.Identity;
using Solution.Core.ParameterModel.ChangeModel.Identity;
using Solution.Core.ParameterModel.QueryModel.Identity;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IRoleService : IServiceSupport
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<long, string>> AddRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> EditRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> RemoveRoleAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Pagination<RoleDTO>> GetRoleAsync(RoleQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDTO> GetRoleByPropAsync(RoleQueryModel queryModel, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDTO> GetRoleByIdAsync(long id);
        /// <summary>
        /// 根据条件获取一条数据及其子集
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDTO> GetRoleAndEmployeesByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据id数组获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<RoleDTO>> GetRoleByIdsAsync(long[] ids);
    }
}
