
namespace Solution.Core.CommonHelper
{
    public class CascadeNavigationAttribute:Attribute
    {
        public string EntityName { get; set; } = null!;
        public CascadeNavigationAttribute(string entityName)
        {
            this.EntityName = entityName;
        }
    }
}
