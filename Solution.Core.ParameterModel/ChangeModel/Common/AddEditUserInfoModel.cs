

namespace Solution.Core.ParameterModel.ChangeModel.Common
{
    public class AddEditUserInfoModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string? Cellphone { get; set; } 
        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }
    }
}
