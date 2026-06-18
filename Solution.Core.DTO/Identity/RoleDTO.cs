
namespace Solution.Core.DTO.Identity
{
    public class RoleDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = null!;
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
        /// <summary>
        /// 角色下的员工
        /// </summary>
        public List<EmployeeDTO>? Employees {  get; set; } 
       
    }
}
