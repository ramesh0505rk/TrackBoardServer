using Microsoft.Extensions.DependencyInjection;
using TrackBoard.Infrastructure.Helpers;
using TrackBoard.Infrastructure.Interfaces;
using TrackBoard.Infrastructure.Presistence;
using TrackBoard.Infrastructure.Repositories;

namespace TrackBoard.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();


            // Register DbConnectionFactory
            services.AddScoped<IDbConnectionFactory,DbConnectionFactory>();

            // Register helpers
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
