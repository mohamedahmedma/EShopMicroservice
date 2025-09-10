
using Ordering.Application.Data;

namespace Ordering.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure
        (this IServiceCollection services , IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        //Add service to the container.
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor , DispatchDomainEventsInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp , options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()!);
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        return services;

    }
}
