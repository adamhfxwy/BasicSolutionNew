
using IsolationLevel = System.Transactions.IsolationLevel;

namespace WebAPI.Filters;

public class TransactionScopeFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		bool hasTransactionalAttribute = false;
		if (context.ActionDescriptor is ControllerActionDescriptor)
		{
			var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
			hasTransactionalAttribute = actionDesc.MethodInfo
				.IsDefined(typeof(TransactionalAttribute));
		}
		if (!hasTransactionalAttribute)
		{
			await next();
			return;
		}
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };
        using var txScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        var result = await next();
		
		if (result.Exception == null)
		{
			txScope.Complete();
		}
	}
}
