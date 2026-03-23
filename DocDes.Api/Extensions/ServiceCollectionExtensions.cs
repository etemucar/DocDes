using DocDes.Core.Data;
using DocDes.Data;
using DocDes.Api.Services;
using DocDes.Core.Services;
using DocDes.Core.Model;
using DocDes.Core.Repository;
using DocDes.Data.Repositories;

namespace DocDes.Api.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}
