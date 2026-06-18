

namespace Solution.Core.ParameterModel.QueryModel.Identity
{
    /// <summary>
    /// 角色（职位）查询模型
    /// </summary>
    public class RoleQueryModel : QueryBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SearchProperty("RoleName")]
        [ContainsProp]
        public string? RoleName { get; set; }
    }
}
