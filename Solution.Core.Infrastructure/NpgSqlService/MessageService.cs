using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Dictionary> _repository;
        public MessageService(IRepository<Dictionary> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AliSendSms(string templatecode, string phone, string param)
        {
            var category = await _repository.GetAllAsync().FirstOrDefaultAsync(x => x.Key == "阿里云短信");
            if (category == null)
            {
                return false;
            }
            var list = await _repository.GetAllAsync().Where(x => x.Type == category.Id).ToListAsync();
            var appKey = list.FirstOrDefault(x => x.Key == "AliAccessKey")?.Value?.Trim();
            var secret = list.FirstOrDefault(x => x.Key == "AliAccessSecret")?.Value?.Trim();
            var signName = list.FirstOrDefault(x => x.Key == "AliSmsSign")?.Value?.Trim();
            bool istrue = false;
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", appKey, secret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = "dysmsapi.aliyuncs.com",
                Version = "2017-05-25",
                Action = "SendSms"
            };

            try
            {

                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.AddQueryParameters("PhoneNumbers", phone);
                //必填:短信签名-可在短信控制台中找到
                request.AddQueryParameters("SignName", signName);
                //必填:短信模板-可在短信控制台中找到
                request.AddQueryParameters("TemplateCode", templatecode);
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.AddQueryParameters("TemplateParam", param);
                //request.TemplateParam = param; //"{\"code\":\"" + "dddd" + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                // request.OutId = "1";
                //请求失败这里会抛ClientException异常
                CommonResponse response = client.GetCommonResponse(request);
                var msg = System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
                if (response.HttpStatus == 200)
                {
                    istrue = true;
                }
            }
            catch (ServerException)
            {
                istrue = false;
            }
            catch (ClientException)
            {
                istrue = false;
            }
            return istrue;
        }   
    }
}
