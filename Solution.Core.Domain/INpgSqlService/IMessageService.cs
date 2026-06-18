using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IMessageService : IServiceSupport
    {
        Task<bool> AliSendSms(string templatecode, string phone, string param);

    }
}
