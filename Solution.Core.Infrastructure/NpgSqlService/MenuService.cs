

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Identity;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _repository;
        public MenuService(IRepository<Menu> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddMenuAsync(AddEditMenuModel model, CancellationToken cancellationToken = default)
        {
            Menu entity = new Menu(model.Path, model.Name, model.MenuName, model.ParentId, model.Description, model.Component, model.Icon
                , model.IsButton.Value, model.MenuCode, model.ButtonName, model.Level);
            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditMenuAsync(AddEditMenuModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的菜单或按钮不存在");
            }
            if (!string.IsNullOrEmpty(model.Path))
            {
                obj.ChangePath(model.Path);
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                obj.ChangeName(model.Name);
            }
            if (!string.IsNullOrEmpty(model.MenuName))
            {
                obj.ChangeMenuName(model.MenuName);
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                obj.ChangeDescription(model.Description);
            }
            if (model.ParentId.HasValue)
            {
                obj.ChangeParentId(model.ParentId.Value);
            }
            if (!string.IsNullOrEmpty(model.Component))
            {
                obj.ChangeComponent(model.Component);
            }
            if (!string.IsNullOrEmpty(model.Icon))
            {
                obj.ChangeIcon(model.Icon);
            }
            if (!string.IsNullOrEmpty(model.MenuCode))
            {
                obj.ChangeMenuCode(model.MenuCode);
            }
            if (!string.IsNullOrEmpty(model.ButtonName))
            {
                obj.ChangeButtonName(model.ButtonName);
            }
            await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        public async Task<List<MenuDTO>> GetMenusByIdsAsync(long[] menuIds, CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<Menu>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Where(x => menuIds.Any(i => i == x.Id)).Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<Menu>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.Where(x => menuIds.Any(i => i == x.Id)).ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        public async Task<List<MenuDTO>> GetAllMenusAsync(CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<Menu>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Where(x => x.IsButton == IsButtonEnum.否).Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<Menu>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.Where(x => x.IsButton == IsButtonEnum.否).ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        public async Task<List<MenuDTO>> GetAllButtonsAsync(CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<Menu>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Where(x => x.IsButton == IsButtonEnum.是).Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<Menu>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.Where(x => x.IsButton == IsButtonEnum.是).ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        public async Task<List<MenuDTO>> GetAllMenusAndButtonAsync(CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<Menu>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<Menu>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        public async Task<List<MenuDTO>> GetMenusByParentIdAsync(long parentId, CancellationToken cancellationToken = default, params string[] param)
        {
            var data = _repository.GetAllAsync().Where(x => x.ParentId == parentId && x.IsDeleted == IsDeletedEnum.未删除);
            var select = SelectUtil.GetSelectCol<Menu>(param);
            if (select != null && param.Length > 0)
            {
                var entity = await data.Select(select).ToListAsync(cancellationToken);
                var result = SelectUtil.GetSelectedEntities<Menu>(entity, param).ToList();
                return result.Select(ToDTOUtils.ToDTO).ToList();
            }
            else
            {
                var entity = await data.ToListAsync(cancellationToken);
                return entity.Select(ToDTOUtils.ToDTO).ToList();
            }
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MenuDTO> GetMenusAndButtonByIdAsync(long id)
        {
            var obj = await _repository.FindAsync(id);
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> RemoveMenusAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的菜单不存在");
            }
            bool exists = await _repository.AnyAsync(x => x.IsDeleted == IsDeletedEnum.未删除 && x.ParentId == obj.Id, cancellationToken);
            if (exists)
            {
                return new Tuple<bool, string>(false, $"id={id}的字典有子集引用，无法删除");
            }
            obj.ChangeIsDeleted();
            await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
    }
}
