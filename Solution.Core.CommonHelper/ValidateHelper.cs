

namespace Solution.Core.CommonHelper
{
    public class ValidateHelper
    {
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckPhoneIsAble(string input)
        {
            if (input.Length < 11)
            {
                return false;
            }
            //电信手机号码正则
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex regexDX = new Regex(dianxin);
            //联通手机号码正则
            string liantong = @"^1[34578][01256]\d{8}";
            Regex regexLT = new Regex(liantong);
            //移动手机号码正则
            string yidong = @"^(1[012345678]\d{8}|1[345678][012356789]\d{8})$";
            Regex regexYD = new Regex(yidong);
            if (regexDX.IsMatch(input) || regexLT.IsMatch(input) || regexYD.IsMatch(input))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckPhoneIsAbleTwo(string input)
        {
            if (input.Length != 11 && input.Length !=7)
            {
                return false;
            }
            foreach (var item in input)
            {
                if (char.IsDigit(item))
                {
                    continue;
                }
                else
                {
                    return false;
                }
             
            }
            return true;
        }
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="len">需要创建验证码的长度</param>
        /// <returns></returns>
        public static string CreateVerifyCode(int len)
        {
            char[] data = { 'a','c','d','e','f','h','k','m',
                'n','r','s','t','w','x','y','A','B','D','E','F','G','H','L','P','Q','R','S','T','U','W','0','1','2','3','4','5','6','7','8','9' };
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = rand.Next(data.Length);//[0,data.length)
                char ch = data[index];
                sb.Append(ch);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 是否是身份证号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckIdCardNumIsAble(string input)
        {
            if (input.Length < 11)
            {
                return false;
            }
            //电信手机号码正则
            string regrx = @"^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|31)|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}([0-9]|x|X)$";
            Regex regexDX = new Regex(regrx);
            if (regexDX.IsMatch(input) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsBase64(String str)
        {
            if (str == null)
            {
                return false ;
            }
            if (str.Contains(','))
            {
                string[] sp = str.Split(',');
                if (sp.Length>1)
                {
                    String base64Pattern = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
                    Regex regexDX = new Regex(base64Pattern);
                    return regexDX.IsMatch(sp[1]);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

          
        }
    }
}
