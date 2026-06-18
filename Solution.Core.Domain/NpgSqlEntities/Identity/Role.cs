

namespace Solution.Core.Domain.NpgSqlEntities.Identity
{
    /// <summary>
    /// 角色（职位）实体
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; private set; } = null!;
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; private set; }

        /// <summary>
        /// web权限项
        /// </summary>
        public string? Permissions { get; private set; }
        /// <summary>
        /// 移动端权限项
        /// </summary>
        public string? MobilePermissions { get; private set; }
        /// <summary>
        /// 员工导航属性
        /// </summary>
        public List<Employee>? Employees { get; private set; }
        private Role()
        {

        }
        public Role(string roleName, long[]? permissions, long[]? mobilePermissions, string? remark)
        {
            RoleName = roleName;
            if (permissions!=null && permissions.Length >0)
            {
                Permissions = string.Join(",", permissions);
            }
          
            Remark = remark;
            //this.CreateTime = DateTime.Now;
            if (mobilePermissions != null && mobilePermissions.Length > 0)
            {
                MobilePermissions = string.Join(",", mobilePermissions);
            }
          
        }
        public void ChangeMobilePermissions(long[] mobilePermissions)
        {
            MobilePermissions = string.Join(",", mobilePermissions);
        }
        public void ChangePermissions(long[] permissions)
        {
            Permissions = string.Join(",", permissions);
        }
        public void ChangeRemark(string remark)
        {
            Remark = remark;
        }
        public void ChangeRoleName(string roleName)
        {
            RoleName = roleName;
        }
    }
}
