﻿using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;

using SmartLock.Application.Consts;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.UI.RestApi.Extensions {
    public static class ApplicationBuilderExtension {
        public static IApplicationBuilder UseSmartLockExceptionHandler(this IApplicationBuilder app) {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null) {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new Result(ResultCode.ServerError, ErrorMessages.InternalServerError)));
                    }
                });
            });

            return app;
        }
    }
}
