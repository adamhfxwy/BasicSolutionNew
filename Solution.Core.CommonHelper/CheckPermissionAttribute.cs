

namespace Solution.Core.CommonHelper
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class CheckPermissionAttribute : Attribute
    {
        public string Permission { get; set; }
        public CheckPermissionAttribute(string permission)
        {

            this.Permission = permission;
        }
    }
}
