

namespace Solution.Core.DTO.Common
{
    public class UserInfoDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Cellphone { get; set; } = null!;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = null!;
       
    }
}
