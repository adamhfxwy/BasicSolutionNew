

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Identity;
using Solution.Core.ParameterModel.QueryModel.Identity;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class RoleService :  IRoleService
    {
        private readonly IRepository<Role> _repository;
        public RoleService(IRepository<Role> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(model.RoleName))
            {
                return new Tuple<long, string>(0, "角色（职位）名称不可为空");
            }
            bool exists = await _repository.AnyAsync(x => x.RoleName == model.RoleName && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "角色（职位）名称不能重复");
            }
            Role entity = new Role(model.RoleName, model.Permissions, model.MobilePermissions, model.Remark);
            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的角色（职位）不存在");
            }

            if (!string.IsNullOrEmpty(model.RoleName))
            {
                bool exists = await _repository.AnyAsync(x => x.RoleName == model.RoleName && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "角色（职位）名称不能重复");
                }
                obj.ChangeRoleName(model.RoleName);
            }
            if (!string.IsNullOrEmpty(model.Remark))
            {

                obj.ChangeRemark(model.Remark);
            }
            if (model.Permissions != null && model.Permissions.Count() > 0)
            {
                obj.ChangePermissions(model.Permissions);
            }
            if (model.MobilePermissions != null && model.MobilePermissions.Count() > 0)
            {
                obj.ChangeMobilePermissions(model.MobilePermissions);
            }
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> RemoveRoleAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的部门不存在");
            }
            obj.ChangeIsDeleted();
            await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Pagination<RoleDTO>> GetRoleAsync(RoleQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Role, RoleQueryModel>(queryModel, cancellationToken);
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<RoleDTO> { List = dto, Total = list.Total, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RoleDTO> GetRoleByPropAsync(RoleQueryModel queryModel, CancellationToken cancellationToken = default)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Role, RoleQueryModel>(queryModel, cancellationToken);
            var obj = await _repository.FindAsync(exp, cancellationToken);
            if (obj != null)
            {
                return ToDTOUtils.ToDTO(obj);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RoleDTO> GetRoleByIdAsync(long id)
        {
            var obj = await _repository.FindAsync(id, x => x.Include(i => i.Employees));
            if (obj != null)
            {
                return ToDTOUtils.ToDTO(obj);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据条件获取一条数据及其子集
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RoleDTO> GetRoleAndEmployeesByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.GetAllAsync().Include(x => x.Employees.Where(x => x.IsDeleted == IsDeletedEnum.未删除))
             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (obj == null)
            {
                return null;
            }
            return ToDTOUtils.ToDTO(obj);
        }
        public async Task<List<RoleDTO>> GetRoleByIdsAsync(long[] ids)
        {
            // 判断是否有重复绑定
            var list = await _repository.GetAllAsync()
                 .Where(x => ids.Any(i => ids.Contains(x.Id)))
                 .ToListAsync();
            return list.Select(ToDTOUtils.ToDTO).ToList();
        }
    }
}
