using Microsoft.OpenApi.Models;
using System.Reflection;
using TestProj.Core.Interfaces;
using TestProj.Infrastructure.Data;
using TestProj.Infrastructure.Repositories;
using TestProj.Infrastructure.Services;

namespace TestProj.Core.Services;

public class ApplicationLog
{
}

public static class ConfigureService
{
    private static string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    private static string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);


    public static void ConfigureServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<ApplicationLog>>();
        services.AddSingleton(typeof(ILogger), logger);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product API",
                Version = "v1",
                Description = "API for managing products in the system"
            });

            options.IncludeXmlComments(xmlPath);
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        #region Registration

        services.AddSingleton<MongoContext>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IFileService, FileService>();

        services.AddScoped<IProductService, ProductService>();

        #endregion
    }
}


