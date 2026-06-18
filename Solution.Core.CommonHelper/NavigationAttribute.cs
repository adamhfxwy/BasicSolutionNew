
namespace Solution.Core.CommonHelper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationAttribute:Attribute
    {
        public string EntityName { get; set; } = null!;
        public NavigationAttribute(string entityName)
        {
            this.EntityName = entityName;
        }
    }
}
