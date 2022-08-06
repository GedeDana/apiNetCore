using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DelegasiAPI.Helpers
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";
        public async Task OnActionExecutionAsync
            (ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var extractedApiKey = context.HttpContext.Request.Headers[APIKEYNAME].FirstOrDefault();
            if (string.IsNullOrEmpty(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key Was Not Provided"
                };

                return;
            }

            var appSetting = context.HttpContext.RequestServices.GetRequiredService<IOptions<AppSettings>>().Value ?? new();

            var apiKey = appSetting.AdminApiKey;

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is Not valid"
                };
                return;
            }
            await next();
        }
    }
}
