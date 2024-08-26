using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CampaignBudgetAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using CampaignBudgetAPI.Validators;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace CampaignBudgetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);

            // Add services to the container.
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<BudgetRequestValidator>();
            builder.Services.AddControllers();
            builder.Services.AddScoped<BudgetCalculator>();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign Budget API V1");
                    c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
                });
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is Exception exception)
                    {
                        var error = new ErrorResponse
                        {
                            Message = "An error occurred while processing your request.",
                            Details = exception.Message
                        };

                        await context.Response.WriteAsJsonAsync(error);
                    }
                });
            });

            app.UseCors("AllowAll");
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Details { get; set; }
    }
}