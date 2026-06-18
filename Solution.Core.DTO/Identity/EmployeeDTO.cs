

namespace Solution.Core.DTO.Identity
{
    public class EmployeeDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = null!;
        /// <summary>
        /// 用户名
        /// </summary>
        public Guid UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Cellphone { get; set; } = null!;
        /// <summary>
        /// 角色（职位）id
        /// </summary>
        public string? RoleId { get; set; }
        /// <summary>
        /// 角色（职位）id数组
        /// </summary>
        public long[]? RoleIds { get; set; }
        /// <summary>
        /// 角色（职位）名称
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// 性别 1-男 2-女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 性别 1-男 2-女
        /// </summary>
        public string? SexStr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限
        /// </summary>
        public int LoginPermissions { get; set; }
        /// <summary>
        /// 登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限
        /// </summary>
        public string? LoginPermissionsStr { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public long? DepartmentId { get; set; }
        /// <summary>
        /// 员工状态 1-正常 2-离职
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 员工状态 1-正常 2-离职
        /// </summary>
        public string? StatusStr { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 照片地址
        /// </summary>
        public string? PhotoPath { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 班次id
        /// </summary>
        public long? ShiftId { get; set; }
        /// <summary>
        /// 班次名称
        /// </summary>
        public string? ShiftName { get; set; }
        /// <summary>
        /// web权限项
        /// </summary>
        public long[]? Permissions { get; set; }
        /// <summary>
        /// 移动端权限项
        /// </summary>
        public long[]? MobilePermissions { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string? JobName { get; set; }

    }
}
