namespace WebAPI.Models
{
    public class ChangePasswordModel
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
    }
}
