
namespace LanTian.Solution.Core.SmartParkMobileAPI.Models
{
    public class MenuTreeModel : IToTreeModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Path { get; set; } = null!;
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = null!;
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? ButtonName { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string? Component { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createtime { get; set; }
        /// <summary>
        /// 菜单或按钮编码
        /// </summary>
        public string MenuCode { get; set; } = null!;
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<IToTreeModel> Children { get; set; }=new List<IToTreeModel>();
       
        public Meta Meta { get; set; }
        /// <summary>
        /// 添加子集
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public void AddChilrden(IToTreeModel node)
        {
            //if (Children == null)
            //    Children = new List<IToTreeModel>();
            this.Children.Add(node);
        }
      
    }
    public class Meta
    {
        /// <summary>
        /// 按钮子集
        /// </summary>
        public List<string> Buttons { get; set; } = new List<string>();
    }
}
