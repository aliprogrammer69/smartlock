

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace SmartLock.UI.RestApi.Configuration {
    public sealed class StatusAwareOutputFormatterSetup : IConfigureOptions<MvcOptions> {
        private readonly JsonOptions _jsonOption;

        public StatusAwareOutputFormatterSetup(IOptions<JsonOptions> jsonOption) {
            if (jsonOption == null)
                throw new ArgumentNullException(nameof(jsonOption));

            this._jsonOption = jsonOption.Value;
        }

        public void Configure(MvcOptions options) {
            IOutputFormatter oldFormatter = options.OutputFormatters.FirstOrDefault(f => f.GetType() == typeof(SystemTextJsonOutputFormatter));
            if (oldFormatter != null) {
                options.OutputFormatters.Remove(oldFormatter);
            }

            options.OutputFormatters.Add(new StatusCodeAwareOutputFormatter(_jsonOption.JsonSerializerOptions));
        }
    }
}
