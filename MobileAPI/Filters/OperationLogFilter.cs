

using Solution.Core.Domain.INpgSqlService;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace MobileAPI.Filters
{
    public class OperationLogFilter : IAsyncActionFilter
    {
        private readonly IDictionaryService _dictionaryService;
        private readonly IOperationLogService _operationLogService;
        //public static event Action<long, string, string, string, string>? LogActionEvent;
        private readonly IServiceScopeFactory _service;
        private IServiceScope _serviceScope;

        public OperationLogFilter(IServiceScopeFactory service)
        {
            _service = service;
            _serviceScope = _service.CreateScope();        
            _dictionaryService = _serviceScope.ServiceProvider.GetRequiredService<IDictionaryService>();
            _operationLogService = _serviceScope.ServiceProvider.GetRequiredService<IOperationLogService>();
        }
        private async Task<bool> AddLog(long empId, string empName, string path, string operName, string requestStr, string responseStr)
        {
            AddEditOperationLogModel model = new AddEditOperationLogModel
            {
                EmpId = empId,
                EmpName = empName,
                OperationName = operName,
                ApiPath = path,
                RequestMessage = requestStr,
                ResponseMessage = responseStr
            };
            var tuple = await _operationLogService.AddOperationLogAsync(model);
            if (tuple.Item1 > 0)
            {
                var res = await _dictionaryService.GetDictionaryByPropAsync(new DictionaryQueryModel { Key = "OperationName" });
                if (res != null)
                {
                    bool exists = await _dictionaryService.AnyAsync(operName, CancellationToken.None);
                    if (!exists)
                    {
                        AddEditDictionaryModel dictionary = new AddEditDictionaryModel
                        {
                            Description = operName,
                            Key = operName,
                            Value = operName,
                            Type = Convert.ToInt32(res.Id)
                        };
                        await _dictionaryService.AddDictionaryAsync(dictionary);
                    }
                }

            }
            return true;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasCheckAccessorAttribute = false;
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
                hasCheckAccessorAttribute = actionDesc.MethodInfo
                    .IsDefined(typeof(OperationLogAttribute), false);
            }
            if (!hasCheckAccessorAttribute)
            {
                await next();
                return;
            }
            ActionExecutedContext result = await next();
            if (result.Exception == null)
            {
                var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
                var path = context.HttpContext.Request.Path;
                OperationLogAttribute operNameAttr = (OperationLogAttribute)descriptor.MethodInfo.GetCustomAttribute(typeof(OperationLogAttribute), false);
                if (operNameAttr != null)
                {
                    string responseStr = string.Empty;
                    string requestStr = string.Empty;
                    string operName = string.Empty;
                    long empId = 0;
                    string empName = string.Empty;
                    string token = string.Empty;

                    string authHeader = context.HttpContext.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer"))
                    {
                        token = authHeader.Substring("Bearer ".Length).Trim();
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;
                        empId = Convert.ToInt64(jwtToken["UserId"]);
                        empName = jwtToken["RealName"].ToString();
                    }
                    operName = operNameAttr.OperationName;
                    IDictionary<string, object> argsMap = context.ActionArguments;
                    foreach (KeyValuePair<string, object> arg in argsMap)
                    {
                        requestStr = JsonConvert.SerializeObject(arg.Value);
                    }
                    var res = (ObjectResult)result.Result!;
                    if (res != null)
                    {
                        responseStr = res.Value.ToString();
                    }
                    var status = res.Value.GetType().GetProperty("Status")?.GetValue(res.Value);
                    if (status == "Ok")
                    {
                        bool isOk = true;
                        while (isOk)
                        {
                            var ok = await AddLog(empId, empName, path, operName.Trim(), requestStr, responseStr);
                            if (ok)
                            {
                                isOk = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
