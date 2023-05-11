using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SmartLock.Application.Configurations.Models;
using SmartLock.Infrastructure.DataBase;

namespace SmartLock.Bootstrapper.Extensions {
    public static class ServiceCollectionExtension {
        public static IServiceCollection AddSmartLock(this IServiceCollection services, IServiceCollectionConfigurationManager configurationManager = null!) {
            configurationManager ??= new DefaultServiceConfigurationManager();
            configurationManager.RegisterRepositories(services)
                                .RegisterServices(services)
                                .RegisterUtils(services);
            return services;
        }

        public static IServiceCollection AddSmartLockDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> options) {
            services.AddDbContext<SmartLockDbContext>(options);
            return services;
        }
    }
}
