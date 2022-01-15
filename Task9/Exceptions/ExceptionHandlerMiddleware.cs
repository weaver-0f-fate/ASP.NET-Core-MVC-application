using System;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Task9.Exceptions {
    public class ExceptionHandlerMiddleware {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next.Invoke(context);
            }
            catch (ForeignEntitiesException) {
                context.Response.Redirect($"{context.Request.Path}?showMessage=True");
            }
            catch (Exception ex) {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception) {
            var response = context.Response;

            response.ContentType = "application/json";

            response.StatusCode = exception switch {
                NoEntityException => (int)HttpStatusCode.NotFound,
                ForeignEntitiesException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            response.ContentType = "application/json";

            return response.WriteAsync($"Status code {response.StatusCode} error. {exception.Message}");

        }
    }
}
