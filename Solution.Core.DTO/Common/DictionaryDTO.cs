

namespace Solution.Core.DTO.Common
{
    public class DictionaryDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = null!;
        /// <summary>
        /// 值 
        /// </summary>
        public string Value { get; set; } = null!;
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 类型 0为创建类型 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? TypeName { get; set; }

    }
}
