

namespace Solution.Core.DTO.Common
{
    public class OperationLogDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public long EmpId { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmpName { get; set; } = null!;
        /// <summary>
        /// 操作项目的名称
        /// </summary>
        public string OperationName { get; set; } = null!;
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiPath { get; set; } = null!;
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
