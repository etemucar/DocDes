using DocDes.Core.Data;
using DocDes.Infrastructure;
using DocDes.Api.Services;
using DocDes.Core.Services;
using DocDes.Core.Security;
using DocDes.Core.Repository;
using DocDes.Infrastructure.Repositories;
using DocDes.Infrastructure.Security;

namespace DocDes.Api.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
