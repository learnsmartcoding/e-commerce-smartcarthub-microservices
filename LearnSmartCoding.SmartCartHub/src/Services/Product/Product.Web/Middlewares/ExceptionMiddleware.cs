using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Products.Web.Middlewares
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; } = string.Empty;
    }
    public class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var errorResponse = new ErrorResponse();
                    if (env.IsDevelopment())
                    {
                        errorResponse.Message = exception?.ToString();
                        errorResponse.StackTrace = exception?.StackTrace;
                    }
                    else
                    {
                        errorResponse.Message = "An unexpected error occurred. Please try again later.";
                    }
                    // Set the Content-Type header to application/json
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(errorResponse);
                    // Serialize and write the errorResponse object to the response stream
                    //await JsonSerializer.SerializeAsync(context.Response.Body, errorResponse);
                });
            });
        }
    }
}
