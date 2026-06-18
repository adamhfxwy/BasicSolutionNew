

namespace Solution.Core.ParameterModel.QueryModel.Common
{
    public class UserInfoQueryModel : QueryBase
    {
        /// <summary>
        ///   
        /// </summary>
        [SearchProperty("Id")]
        public long? Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [SearchProperty("Name")]
        public string? Name { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [SearchProperty("Cellphone")]
        public string? Cellphone { get; set; }

        [SearchDateEndLessThanOrEqual]
        [SearchProperty("CreateTime")]
        public string? RegisterDate {  get; set; }
    }
}
