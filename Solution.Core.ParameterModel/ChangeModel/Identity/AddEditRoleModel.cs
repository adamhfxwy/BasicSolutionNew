

namespace Solution.Core.ParameterModel.ChangeModel.Identity
{
    /// <summary>
    /// 角色（职位）新增编辑模型
    /// </summary>
    public class AddEditRoleModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// web权限项
        /// </summary>
        public long[]? Permissions { get; set; }
        /// <summary>
        /// 移动端权限项
        /// </summary>
        public long[]? MobilePermissions { get; set; }
    }
}
