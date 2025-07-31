namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Register API services here
            // Example: services.AddControllers();
            // Example: services.AddSwaggerGen();

            // Register application and infrastructure services
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure API middleware here
            // Example: app.UseSwagger();
            // Example: app.UseAuthorization();

            // Map controllers
            //app.MapControllers();

            return app;
        }
    }
}
