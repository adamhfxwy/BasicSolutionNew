

namespace Solution.Core.ParameterModel.QueryModel
{
    public abstract class QueryBase
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// 排序字段（可选）
        /// </summary>
        public string? OrderBy { get; set; } = "createTime";
        /// <summary>
        /// 是否降序，true:降序   false:升序
        /// </summary>
        public bool IsDescending { get; set; } = true;
        /// <summary>
        /// 软删字段（无需传参）
        /// </summary>
        [SearchProperty("IsDeleted")]
        public IsDeletedEnum IsDeleted { get; set; } = IsDeletedEnum.未删除;
    }
}
