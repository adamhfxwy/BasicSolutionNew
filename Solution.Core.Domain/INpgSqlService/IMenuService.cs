

using Solution.Core.DTO.Identity;
using Solution.Core.ParameterModel.ChangeModel.Identity;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IMenuService : IServiceSupport
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<long, string>> AddMenuAsync(AddEditMenuModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> EditMenuAsync(AddEditMenuModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据id数组获取数据
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuDTO>> GetMenusByIdsAsync(long[] menuIds, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuDTO>> GetAllMenusAsync(CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 获取所有按钮
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuDTO>> GetAllButtonsAsync(CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 获取所有菜单按钮
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuDTO>> GetAllMenusAndButtonAsync(CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据父级id获取所有子集
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuDTO>> GetMenusByParentIdAsync(long parentId, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MenuDTO> GetMenusAndButtonByIdAsync(long id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> RemoveMenusAsync(long id, CancellationToken cancellationToken = default);
    }
}
