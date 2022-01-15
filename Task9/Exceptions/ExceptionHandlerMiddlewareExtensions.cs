using Microsoft.AspNetCore.Builder;

namespace Task9.Exceptions {
    public static class ExceptionHandlerMiddlewareExtensions {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
