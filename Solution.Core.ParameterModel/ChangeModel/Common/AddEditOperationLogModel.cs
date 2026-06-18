

namespace Solution.Core.ParameterModel.ChangeModel.Common
{
    public class AddEditOperationLogModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public long? EmpId { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string? EmpName { get; set; }
        /// <summary>
        /// 操作项目的名称
        /// </summary>
        public string? OperationName { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? ApiPath { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestMessage { get; set; }
        /// <summary>
        /// 响应参数
        /// </summary>
        public string? ResponseMessage { get; set; }
    }
}
