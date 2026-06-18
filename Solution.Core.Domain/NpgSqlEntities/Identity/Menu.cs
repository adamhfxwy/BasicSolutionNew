

namespace Solution.Core.Domain.NpgSqlEntities.Identity
{
    public class Menu : BaseEntity
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string? Path { get; private set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; private set; } = null!;
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? ButtonName { get; private set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; private set; } = null!;
        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; private set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; private set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string? Component { get; private set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; private set; }
        /// <summary>
        /// 是否是按钮 1-否 2-是
        /// </summary>
        public IsButtonEnum IsButton { get; init; }
        /// <summary>
        /// 菜单或按钮编码
        /// </summary>
        public string? MenuCode { get; private set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Level {  get; init; }
        private Menu()
        {

        }
        public Menu(string path, string name, string menuName, long? parentId, string? description, string? component
            , string? icon, IsButtonEnum isButton, string menuCode, string? buttonName,int level)
        {
            this.Path = path;   
            this.Name = name;
            this.MenuName = menuName;
            this.ParentId = parentId;
            this.Description = description;
            this.Component = component;
            this.Icon = icon;
            this.IsButton = isButton;
            if (level!=1)
            {
                this.MenuCode = menuCode;
            }         
            this.ButtonName = buttonName;
            this.Level = level;
        }
        public void ChangePath(string path)
        {
            this.Path = path;
        }
        public void ChangeName(string name)
        {
            this.Name = name;
        }
        public void ChangeMenuName(string menuName)
        {
            this.MenuName = menuName;
        }
        public void ChangeParentId(long parentId)
        {
            this.ParentId = parentId;
        }
        public void ChangeDescription(string description)
        {
            this.Description = description;
        }
        public void ChangeComponent(string component)
        {
            this.Component = component;
        }
        public void ChangeIcon(string icon)
        {
            this.Icon = icon;
        }
        public void ChangeMenuCode(string menuCode)
        {
            this.MenuCode = menuCode;
        }
        public void ChangeButtonName(string buttonName)
        {
            this.ButtonName = buttonName;
        }
    }
}
