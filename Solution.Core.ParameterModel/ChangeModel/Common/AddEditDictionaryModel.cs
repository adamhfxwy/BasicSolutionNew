

namespace Solution.Core.ParameterModel.ChangeModel.Common
{
    public class AddEditDictionaryModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string? Key { get; set; } 
        /// <summary>
        /// 值 
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 类型(创建类型时无需赋值，创建值时传所选类型id)
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 是否是创建类型 false=不是类型  true=是类型
        /// </summary>
        public bool? IsCreateType { get; set; }

    }
}
