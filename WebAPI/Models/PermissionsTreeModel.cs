
namespace WebAPI.Models
{
    public class PermissionsTreeModel : IToTreeModel
    {
        /// <summary>
        /// id主键
        /// </summary>
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
        public long ParentId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 权限项编码
        /// </summary>
        public string? PermissionCode { get; set; }
        /// <summary>
        /// 软删  1-未删除 2-已删除
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<IToTreeModel> Children { get; set; } = new List<IToTreeModel>();

        public void AddChilrden(IToTreeModel node)
        {
            this.Children.Add(node);
        }
    }
}
