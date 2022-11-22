using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Budgeteer.Server.OpenTelemetry;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, string serviceName)
    {
        services
            .AddOpenTelemetryMetrics(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddPrometheusExporter()
                    .AddConsoleExporter()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddProcessInstrumentation()
                    .AddEventCountersInstrumentation(counters => counters.AddEventSources(
                        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                        // "System.Runtime",
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Http.Connections",
                        // "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.NameResolution",
                        "System.Net.Security",
                        "System.Net.Sockets")
                    );
            });

        return services;
    }
}
