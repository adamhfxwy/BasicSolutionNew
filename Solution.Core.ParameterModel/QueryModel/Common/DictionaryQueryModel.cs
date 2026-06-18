

namespace Solution.Core.ParameterModel.QueryModel.Common
{
    public class DictionaryQueryModel:QueryBase
    {
        /// <summary>
        /// 键
        /// </summary>
        [SearchProperty("Key")]
        
        public string? Key { get; set; }
        /// <summary>
        /// 类型 0为创建类型 
        /// </summary>
        [SearchProperty("Type")]
        public int? Type { get; set; }
        /// <summary>
        /// 无需传参
        /// </summary>
        [SearchProperty("Value")]
        public string? Value { get; set; }
    }
}
