

namespace Solution.Core.CommonHelper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchPropertyAttribute:Attribute
    {
        public string PropertyName { get; set; } = null!;
        public SearchPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

    }
}
