
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace MobileAPI.Filters;
public class MyActionFilter : IAsyncActionFilter
{
    private readonly Logger _nlog;

    public MyActionFilter()
    {
        _nlog = LogManager.GetCurrentClassLogger();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var path = context.HttpContext.Request.Path;
      

        _nlog.Info($"apiStart:{path}");
        ActionExecutedContext result= await next();
        if (result.Exception == null)
        {
            _nlog.Info($"apiEnd:{path}");
        }
        else
        {
            var qs = context.HttpContext.Request.QueryString.Value;
            if (qs != null)
            {
                _nlog.Error($"{path}参数为：{qs}。");
                return;
            }
            IDictionary<string, object> argsMap = context.ActionArguments;
            string paramsStr = string.Empty;
            foreach (KeyValuePair<string, object> arg in argsMap)
            {
                var s = arg.Value;
                if (s != null)
                {
                    Type type = s.GetType();
                    //var obj = Activator.CreateInstance(type, null);
                    if (type.FullName.Contains("System.Collections.Generic.List"))
                    {
                        paramsStr = type.FullName;
                        break;
                    }
                    else
                    {
                        foreach (var prop in type.GetProperties())
                        {
                            var pValue = prop.GetValue(s);
                            if (pValue != null && pValue.ToString().Contains("base64,"))
                            {
                                pValue = "data:image/jpeg;base64";
                            }
                            paramsStr += $"{prop.Name}-{pValue};";// pValue == null? $"{prop.Name}- null ;":$"{prop.Name}-{pValue};";
                            if (paramsStr.Contains("FormFileCollection"))
                            {
                                break;
                            }
                        }
                    }
                }

            }
            _nlog.Info($"{path}参数为：{paramsStr}。");
        }
    }
}
