

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class MobilePermissionsService : IMobilePermissionsService
    {
        private readonly IRepository<MobilePermissions> _repository;
        public MobilePermissionsService(IRepository<MobilePermissions> repository)
        {
            _repository = repository;
        }
        public async Task<List<MobilePermissionsDTO>> GetPermissionsByIdsAsync(long[] permissionIds, CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<MobilePermissions>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Where(x => permissionIds.Any(i => i == x.Id)).Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<MobilePermissions>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.Where(x => permissionIds.Any(i => i == x.Id)).ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        public async Task<List<MobilePermissionsDTO>> GetAllPermissionsAsync(CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<MobilePermissions>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<MobilePermissions>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }

        }
    }
}
