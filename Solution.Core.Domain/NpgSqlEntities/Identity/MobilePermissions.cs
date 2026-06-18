

namespace Solution.Core.Domain.NpgSqlEntities.Identity
{
    public class MobilePermissions : BaseEntity
    {
        /// <summary>
        /// 权限项名称
        /// </summary>
        public string PermissionName { get; init; } = null!;
        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; init; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; init; }
        /// <summary>
        /// 权限项编码
        /// </summary>
        public string? PermissionCode { get; init; }
        private MobilePermissions()
        {

        }
    }
}
