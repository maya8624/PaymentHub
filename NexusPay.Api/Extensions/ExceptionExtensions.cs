using Microsoft.AspNetCore.Mvc;
using NexusPay.Api.Middleware;
using NexusPay.Application.Dtos;
using NexusPay.Application.Exceptions;

namespace NexusPay.Api.Extensions
{
    public static class ExceptionExtensions
    {
        public static (int StatusCode, ErrorResponse Response) ToHttpResponse(this Exception exception, HttpContext context)
        {
            return exception switch
            {
                AppException appEx => (
                    appEx.StatusCode,
                     new ErrorResponse
                     {
                         Name = appEx.Name,
                         Code = appEx.StatusCode,
                         Message = appEx.Message,
                     }),
                NetworkException netEx => (
                   netEx.StatusCode,
                    new ErrorResponse
                    {
                        Name = netEx.Name,
                        Code = netEx.StatusCode,
                        Message = netEx.Message,
                    }),
                _ => (
                   StatusCodes.Status500InternalServerError,
                   new ErrorResponse
                   {
                       Name = "UNEXPECTED_ERROR",
                       Code = StatusCodes.Status500InternalServerError,
                       Message = "An unexpected error occurred.",
                   }
                )
            };
        }
    }
}
