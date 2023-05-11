using Microsoft.Extensions.DependencyInjection;

namespace SmartLock.Application.Configurations {
    public static class MediatRConfiguration {
        public static void ConfigureMediatR(MediatRServiceConfiguration configuration) {
            configuration.RegisterServicesFromAssemblies(typeof(MediatRConfiguration).Assembly);
        }
    }
}
