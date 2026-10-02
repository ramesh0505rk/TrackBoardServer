using Microsoft.Extensions.DependencyInjection;
using TrackBoard.Application.Interfaces;
using TrackBoard.Application.Services;

namespace TrackBoard.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
