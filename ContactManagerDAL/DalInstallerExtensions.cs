using ContactManagerDAL.Interfaces;
using ContactManagerDAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManagerDAL;

public static class DalInstallerExtensions
{
    public static IServiceCollection AddDalLayer(this IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
        return services;
    }
}