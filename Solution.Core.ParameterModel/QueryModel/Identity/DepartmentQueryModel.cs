

namespace Solution.Core.ParameterModel.QueryModel.Identity
{
    /// <summary>
    /// 部门查询模型
    /// </summary>
    public class DepartmentQueryModel : QueryBase
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [SearchProperty("DepartmentName")]
        [ContainsProp]
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 部门负责人id
        /// </summary>
        [SearchProperty("LeaderId")]
        public long? LeaderId { get; set; }
    }
}
