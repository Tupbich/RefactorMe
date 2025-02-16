using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RefactorMe.Dal;

namespace RefactorMe.MsSql.Dal;

public static class IConfigurationExtensions
{
    public static void AddRefactorMeDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);  // Use SQL Server with the connection string
        });
    }
}