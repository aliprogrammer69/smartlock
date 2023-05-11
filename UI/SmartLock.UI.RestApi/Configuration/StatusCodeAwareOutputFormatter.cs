using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Formatters;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.UI.RestApi.Configuration {
    public sealed class StatusCodeAwareOutputFormatter : SystemTextJsonOutputFormatter {
        public StatusCodeAwareOutputFormatter(JsonSerializerOptions jsonSerializerOptions) : base(jsonSerializerOptions) {
        }

        public override Task WriteAsync(OutputFormatterWriteContext context) {
            if (context.Object is Result data)
                context.HttpContext.Response.StatusCode = (int)data.Code;

            return base.WriteAsync(context);
        }
    }
}
