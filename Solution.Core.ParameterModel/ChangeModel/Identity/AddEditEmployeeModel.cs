

namespace Solution.Core.ParameterModel.ChangeModel.Identity
{
    /// <summary>
    /// 员工的新增编辑模型
    /// </summary>
    public class AddEditEmployeeModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string? RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Cellphone { get; set; }
        /// <summary>
        /// 角色（职位）id
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// 性别 1-男 2-女
        /// </summary>
        public SexEnum? Sex { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限
        /// </summary>
        public LoginPermissionsEnum? LoginPermissions { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public long? DepartmentId { get; set; }
        /// <summary>
        /// 员工状态 1-正常 2-离职
        /// </summary>
        public EmployeeStatusEnum? Status { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 照片地址
        /// </summary>
        public string? PhotoPath { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 班次id
        /// </summary>
        public long? ShiftId { get; set; }
        /// <summary>
        /// 用户名(无需传参)
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 角色id数组
        /// </summary>
        public long[]? RoleIds { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string? JobName { get; set; }
    }
}
