

using Solution.Core.DTO.Identity;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IMobilePermissionsService : IServiceSupport
    {
        /// <summary>
        /// 根据id数组获取数据
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MobilePermissionsDTO>> GetPermissionsByIdsAsync(long[] permissionIds, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MobilePermissionsDTO>> GetAllPermissionsAsync(CancellationToken cancellationToken = default, params string[] param);
    }
}
