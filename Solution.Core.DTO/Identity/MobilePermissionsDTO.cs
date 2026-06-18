

namespace Solution.Core.DTO.Identity
{
    public class MobilePermissionsDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 权限项名称
        /// </summary>
        public string PermissionName { get; set; } = null!;
        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 权限项编码
        /// </summary>
        public string? PermissionCode { get; set; }
       
    }
}
