using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TestProj.API.Middlewares;
using TestProj.Core.Interfaces;
using TestProj.Core.Services;
using TestProj.Infrastructure.Data;
using TestProj.Infrastructure.Repositories;
using TestProj.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting up the application");

    builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("DBSettings"));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CORS", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.ConfigureServices();

    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestProj API v1");
        });
    }


    app.UseStaticFiles();
    app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSerilogRequestLogging();

    app.UseCors("CORS");

    app.UseAuthorization();
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}