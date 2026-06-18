
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace MobileAPI.Filters;

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
		using var txScope =
				new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		var result = await next();
		
		if (result.Exception == null)
		{
			txScope.Complete();
		}
	}
}
