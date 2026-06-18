

namespace Solution.Core.ParameterModel.ChangeModel.Identity
{
    public class AddEditMenuModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? Name { get; set; } 
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuName { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string? Component { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 是否是按钮 1-否 2-是
        /// </summary>
        public IsButtonEnum? IsButton { get; set; }
        /// <summary>
        /// 菜单或按钮编码
        /// </summary>
        public string? MenuCode { get; set; }
        /// <summary>
        /// 按钮名称 (编辑时无需传参)
        /// </summary>
        public string? ButtonName { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }
    }
}
