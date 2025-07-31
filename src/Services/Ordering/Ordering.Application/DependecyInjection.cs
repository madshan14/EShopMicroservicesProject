using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services here
            // Example: services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
