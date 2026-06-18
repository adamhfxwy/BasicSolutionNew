

namespace Solution.Core.ParameterModel.QueryModel.Common
{
    public class ShiftInfoQueryModel:QueryBase
    {
        /// <summary>
        /// 班次名称
        /// </summary>
        [SearchProperty("ShiftName")]
        public string? ShiftName { get; set; }
    }
}
