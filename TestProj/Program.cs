using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TestProj.API.Middlewares;
using TestProj.Core.Interfaces;
using TestProj.Infrastructure.Data;
using TestProj.Infrastructure.Repositories;
using TestProj.Infrastructure.Utilities;

var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting up the application");
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    builder.Services.AddControllers();

    builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("DBSettings"));
    builder.Services.AddSingleton<MongoContext>();

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IFileUtility, FileUtility>();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Product API",
            Version = "v1",
            Description = "API for managing products in the system"
        });

        options.IncludeXmlComments(xmlPath);
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CORS", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Host.UseSerilog();

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