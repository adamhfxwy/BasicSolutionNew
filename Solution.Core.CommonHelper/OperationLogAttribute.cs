
namespace Solution.Core.CommonHelper
{
    /// <summary>
    /// 在需要记录操作记录信息时使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OperationLogAttribute : Attribute
    {
        public string OperationName { get; set; }

        public OperationLogAttribute(string operationName)
        {
            OperationName = operationName;
        }
    }
}
