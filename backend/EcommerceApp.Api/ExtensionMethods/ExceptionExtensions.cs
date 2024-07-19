using EcommerceApp.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace EcommerceApp.Api.ExtensionMethods
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(new
            {
                status = StatusCode,
                message = Message
            });
        }
    }

    public static class ExceptionExtensions
    {
        public static void UseCustomExceptionHandler(this WebApplication app)
        {

            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            _ when IsBadRequestException(contextFeature.Error) => StatusCodes.Status400BadRequest,
                            _ when IsNotFoundException(contextFeature.Error) => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };


                        await context.Response.WriteAsync(new ErrorDetail()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }

        private static bool IsBadRequestException(Exception exception)
        {
            if (exception.GetType().IsGenericType && exception.GetType().GetGenericTypeDefinition() == typeof(BadRequestException<>))
            {
                return true;
            }
            return false;
        }

        private static bool IsNotFoundException(Exception exception)
        {
            if (exception.GetType().IsGenericType && exception.GetType().GetGenericTypeDefinition() == typeof(NotFoundException<,>))
            {
                return true;
            }
            return false;
        }
    }
}