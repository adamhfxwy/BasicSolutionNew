

namespace Solution.Core.ParameterModel.QueryModel.Identity
{
    /// <summary>
    /// 员工查询模型
    /// </summary>
    public class EmployeeQueryModel : QueryBase
    {
        /// <summary>
        /// id
        /// </summary>
        [SearchProperty("Id")]
        public long? Id { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [SearchProperty("RealName")]
        [ContainsProp]
        public string? RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [SearchProperty("Cellphone")]
        public string? Cellphone { get; set; }
        /// <summary>
        /// 角色（职位）id
        /// </summary>
        [SearchProperty("RoleId")]
        public long? RoleId { get; set; }
        /// <summary>
        /// 登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限
        /// </summary>
        [SearchProperty("LoginPermissions")]
        public LoginPermissionsEnum? LoginPermissions { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        [SearchProperty("DepartmentId")]
        public long? DepartmentId { get; set; }
        /// <summary>
        /// 员工状态 1-正常 2-离职
        /// </summary>
        [SearchProperty("Status")]
        public EmployeeStatusEnum? Status { get; set; }
        /// <summary>
        /// 班次id
        /// </summary>
        [SearchProperty("ShiftId")]
        public long? ShiftId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [SearchProperty("UserName")]
        public string? UserName { get; set; }
    }
}
