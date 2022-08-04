using Auth.DTO.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Auth.WebApi.Common
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string msg)
        {
            var result = JsonConvert.SerializeObject(new ResponseModel { Code = ResponseCode.Error, Message = msg }, Formatting.Indented,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
