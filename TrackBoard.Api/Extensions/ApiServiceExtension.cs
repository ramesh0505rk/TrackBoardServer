namespace TrackBoard.Api.Extensions
{
    public static class ApiServiceExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCorsExtension(configuration);
            services.AddAuthentication(configuration);
            return services;
        }
    }
}
