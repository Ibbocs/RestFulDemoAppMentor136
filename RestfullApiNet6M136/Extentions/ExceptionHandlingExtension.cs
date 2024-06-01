using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mime;
using System.Text.Json;
using RestfullApiNet6M136.Models;
using Serilog;

namespace RestfullApiNet6M136.Extentions
{
    public static class ExceptionHandlingExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app /*, ILogger logger*/)
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
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        Log.Error($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = $"Internal Server Error: {contextFeature.Error}"
                        }));
                    }
                });
            });
        }
    }
}