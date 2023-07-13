using DataAccessEF.Services.LogFile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using Domain.Shared;

namespace WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, LogFileService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogException($"From UseExceptionHandler Middelware:-\n {contextFeature.Error}");
                        await context.Response.WriteAsync(((int)TaskValidationKeysEnum.UnhandeledException).ToString());
                    }
                });
            });
        }


    }
}
