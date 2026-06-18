

namespace Solution.Core.ParameterModel.ChangeModel.Common
{
    public class ShiftInfoChangeModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 班次名称
        /// </summary>
        public string? ShiftName { get; set; } 
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
