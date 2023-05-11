using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using SmartLock.Application.Configurations.Models;
using SmartLock.Bootstrapper.Extensions;
using SmartLock.Infrastructure.DataBase.Configurations;
using SmartLock.Infrastructure.DataBase.Configurations.Options;
using SmartLock.Shared.Options;
using SmartLock.UI.RestApi.Configuration;
using SmartLock.UI.RestApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", false, true);

builder.Services.AddControllers();

builder.Services.AddSmartLock()
                .AddSmartLockDbContext(opt => {
                    DataBaseOptions dbOption = builder.Configuration.GetOptions<DataBaseOptions>("databaseConfig");
                    SmartLockDbContextConfiguration.ConfigSqlServerDbContext(opt, dbOption);
                    //SmartLockDbContextConfiguration.ConfigInMemoryDbContext(opt);
                })
                .AddSmartLockAuthentication(builder.Configuration.GetOptions<Audience>("audience"))
                .AddSmartLockAuthorization();

//builder.Logging.AddNLog("NLog.config");
//builder.Host.UseNLog();

builder.Services.AddTransient<IConfigureOptions<MvcOptions>, StatusAwareOutputFormatterSetup>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseSmartLockExceptionHandler();

app.UseAutoMigrateSQLServerDataBase(); //In order to use Inmemory dbcontext, comment this line of code

app.UseAuthorization();

app.MapControllers();

app.Run();
