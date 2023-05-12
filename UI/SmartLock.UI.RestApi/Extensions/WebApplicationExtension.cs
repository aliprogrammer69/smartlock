using SmartLock.Infrastructure.DataBase;

namespace SmartLock.UI.RestApi.Extensions {
    public static class WebApplicationExtension {
        public static WebApplication UseAutoMigrateRelationalDataBase(this WebApplication app) {
            app.Services.GetService<SqlServerInitializer>().AutoMigrate();
            return app;
        }
    }
}
