namespace WebAPI.Filters;
public class MyExceptionFilter : IAsyncExceptionFilter
{
    private readonly Logger _nlog;

    public MyExceptionFilter()
    {
          _nlog = LogManager.GetCurrentClassLogger();
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        //context.Exception代表异常信息对象
        //context.ExceptionHandled赋值为true，则其他ExceptionFilter不会再执行
        //context.Result的值会被输出到客户端
        var path=context.HttpContext.Request.Path;
        _nlog.Error($"apiEnd:{path}接口异常：{context.Exception}");
        string msg = context.Exception.ToString();           
        ObjectResult objectResult = new ObjectResult(new { code = 500, message = msg });
        context.Result = objectResult;
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}
