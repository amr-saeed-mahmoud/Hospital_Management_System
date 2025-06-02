using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Data;

namespace Project.Extentions;

public static class EFCoreExtensions
{
    public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(config.GetConnectionString("MyConnectionString"))
        );  
        return services;
    }
    public static WebApplication RecreateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {   
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        return app;
    }
}