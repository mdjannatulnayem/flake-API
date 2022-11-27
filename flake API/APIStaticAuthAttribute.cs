using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace flake_API;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

public class APIStaticAuthAttribute : Attribute,IAsyncActionFilter
{
    private readonly string apiKeyHeaderName = "authKey";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if(!context.HttpContext.Request.Headers.TryGetValue(apiKeyHeaderName,out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        var conf = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = conf.GetValue<string>("APIStaticAuthToken");
        if (!apiKey.Equals(potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        await next();
    }
}
