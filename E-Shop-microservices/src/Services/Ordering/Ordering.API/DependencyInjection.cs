using Carter;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices
            (this IServiceCollection services)
        {
            services.AddCarter();
            return services;
        }

        public static WebApplication UseApiSerivces(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
