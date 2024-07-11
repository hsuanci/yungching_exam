using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using YungChingExam.Middlewares;
using YungChingExam.ViewModel;

namespace YungChingExam.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 任務調用
        /// </summary>
        /// <param name="context">HTTP 的上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // TODO: 可以記錄至DB錯誤資訊。
            context.Response.ContentType = "application/json";
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            _logger.LogError(exception, "An unexpected error occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var responseModel = new ErrorResponseViewModel
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Details = exception.Message
            };

            var jsonResponse = JsonConvert.SerializeObject(responseModel, settings);

            return context.Response.WriteAsync(jsonResponse);
        }
    }

}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
