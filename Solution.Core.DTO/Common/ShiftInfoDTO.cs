

namespace Solution.Core.DTO.Common
{
    public class ShiftInfoDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { get; set; } = null!;
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeOnly BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TimeOnly EndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    
    }
}
