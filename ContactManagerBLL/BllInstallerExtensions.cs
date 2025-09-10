using ContactManagerBLL.Interfaces;
using ContactManagerBLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManagerBLL;

public static class BllInstallerExtensions
{
    public static IServiceCollection AddBllLayer(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        return services;
    }
}