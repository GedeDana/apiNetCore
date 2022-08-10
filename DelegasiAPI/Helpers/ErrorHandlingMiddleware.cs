using DelegasiAPI.Exceptions;
using DelegasiAPI.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Serialization;
using System.Net;
using Serilog;

namespace DelegasiAPI.Helpers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var header = context.Request.Headers.Where(x => x.Key.ToUpper().Contains("APIKEY"));

                var requestBody = await ReadBodyFromRequest(context.Request);

                var logMessage = $"HTTP request information:\n" +
                $"\tMethod: {context.Request.Method}\n" +
                $"\tPath: {context.Request.Path}\n" +
                $"\tQueryString: {context.Request.QueryString}\n" +
                $"\tHeaders: {FormatHeaders(header)}\n" +
                $"\tSchema: {context.Request.Scheme}\n" +
                $"\tHost: {context.Request.Host}\n" +
                $"\tBody: {requestBody}";


                Log.Error(ex, logMessage);


                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            ErrorResponse error;

            if (ex is DefaultException defaultException)
            {
                error = defaultException.Error;
            }
            else
            {
                error = new ErrorResponse(ex.Message, new List<string>(), attachment: ex);
            }

            var result = JsonConvert.SerializeObject(error, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }

        private static string FormatHeaders(IEnumerable<KeyValuePair<string, StringValues>> headers)
        {
            return string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
        }

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
