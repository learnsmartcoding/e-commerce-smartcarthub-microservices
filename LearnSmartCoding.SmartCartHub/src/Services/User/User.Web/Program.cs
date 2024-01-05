using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Templates;
using System.Net;
using System.Text.Json.Serialization;
using User.Data;
using User.Service;
using User.Web.Common;
using User.Web.Middlewares;

namespace User.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;
                // Add services to the container.

                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                 .ReadFrom.Configuration(context.Configuration)
                 .ReadFrom.Services(services)
                 .WriteTo.Console(new ExpressionTemplate(
                     // Include trace and span ids when present.
                     "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                 .WriteTo.ApplicationInsights(
                   services.GetRequiredService<TelemetryConfiguration>(),
                   TelemetryConverter.Traces));

                Log.Information("Starting the application UserProfile Service...");


                builder.Services.AddControllers()
                      .AddJsonOptions(options =>
                      {
                          // Ignore self reference loop
                          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                      });

                // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>

                        {
                            configuration.Bind("AzureAdB2C", options);
                            options.Events = new JwtBearerEvents();

                            /// <summary>
                            /// Below you can do extended token validation and check for additional claims, such as:
                            ///
                            /// - check if the caller's account is homed or guest via the 'acct' optional claim
                            /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
                            ///
                            /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
                            /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
                            /// </summary>

                            //options.Events.OnTokenValidated = async context =>
                            //{
                            //    string[] allowedClientApps = { /* list of client ids to allow */ };

                            //    string clientAppId = context?.Principal?.Claims
                            //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                            //    if (!allowedClientApps.Contains(clientAppId))
                            //    {
                            //        throw new System.Exception("This client is not authorized");
                            //    }
                            //};
                        }, options => { configuration.Bind("AzureAdB2C", options); });

                // The following flag can be used to get more descriptive errors in development environments
                IdentityModelEventSource.ShowPII = false;

                // In production, modify this with the actual domains you want to allow
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                //before this step, install dotnet EF and run commands to pull db entities. E.g. scaffold
                builder.Services.AddDbContext<LearnSmartDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DbContext")
                    )
               //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)            
               .EnableSensitiveDataLogging() //should not be used in production, only for development purpose
               );

                builder.Services.AddScoped<IUserProfileService, UserProfileService>();
                builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
                builder.Services.AddScoped<IAddressService, AddressService>();
                builder.Services.AddScoped<IAddressRepository, AddressRepository>();
                builder.Services.AddScoped<IUserActivityLogService, UserActivityLogService>();
                builder.Services.AddScoped<IUserActivityLogRepository, UserActivityLogRepository>();
                builder.Services.AddScoped<IUserClaims, UserClaims>();

                builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
                builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();


                var app = builder.Build();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });


                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                // Enable our custom middleware
                app.UseMiddleware<RequestBodyLoggingMiddleware>();
                app.UseMiddleware<ResponseBodyLoggingMiddleware>();

                app.UseCors("default");


                // Configure the HTTP request pipeline.
                //if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();               


                app.MapControllers();

                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
    }
}
