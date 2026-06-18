

namespace Solution.Core.ParameterModel.QueryModel.Common
{
    public class OperationLogQueryModel:QueryBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [SearchProperty("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 操作项目的名称
        /// </summary>
        [SearchProperty("OperationName")]
        public string? OperationName { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [SearchProperty("EmpName")]
        public string? EmpName { get; set; }
    }
}
