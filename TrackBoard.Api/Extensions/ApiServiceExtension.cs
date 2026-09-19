using TrackBoard.Application.Extensions;
using TrackBoard.Infrastructure.Extensions;

namespace TrackBoard.Api.Extensions
{
    public static class ApiServiceExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCorsExtension(configuration);
            services.AddAuthentication(configuration);
            services.AddApplicationServices();
            services.AddInfrastructureServices();
            return services;
        }
    }
}
