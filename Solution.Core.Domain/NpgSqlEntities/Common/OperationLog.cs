

namespace Solution.Core.Domain.NpgSqlEntities.Common
{
    public class OperationLog:BaseEntity
    {
        /// <summary>
        /// 员工id
        /// </summary>
        public long EmpId { get; init; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmpName { get; init; } = null!;
        /// <summary>
        /// 操作项目的名称
        /// </summary>
        public string OperationName { get; init; } = null!;
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiPath { get; init; } = null!;
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestMessage { get; init; }
        /// <summary>
        /// 响应参数
        /// </summary>
        public string? ResponseMessage { get; init; }
        private OperationLog()
        {

        }
        public OperationLog(long empId, string empName, string operationName, string apiPath, string? requestMessage, string? responseMessage)
        {
            this.EmpId = empId;
            this.EmpName = empName;
            this.OperationName = operationName;
            this.ApiPath = apiPath;
            this.RequestMessage = requestMessage;
            this.ResponseMessage = responseMessage;
        }
    }
}
