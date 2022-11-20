using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services
    .AddLogging((options) =>
    {
        options.ClearProviders();
        options.AddConsole();
    });

// Options
builder.Services
    .Configure<PlaygroundOptions>(builder.Configuration);
var playgroundOptions = builder.Configuration.Get<PlaygroundOptions>()!;

// Database
builder.Services
    .AddPooledDbContextFactory<PlaygroundContext>((options) => options.UseInMemoryDatabase(playgroundOptions.Database.DatabaseName))
    .AddScoped((provider) => provider.GetRequiredService<IDbContextFactory<PlaygroundContext>>().CreateDbContext());

// Authentication
builder.Services
    .AddAuthentication()
    .AddJwtBearer((options) =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = playgroundOptions.Jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = playgroundOptions.Jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(playgroundOptions.Jwt.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization
builder.Services
    .AddAuthorization();

// AutoMapper
builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Service
builder.Services
    .AddScoped<UserService>()
    .AddScoped<SessionService>()
    .AddScoped<NotificationService>()
    .AddHostedService<BackgroundWorkerService>()
    .AddSingleton<BackgroundQueueService>();

// SignalR
builder.Services
    .AddSignalR();

// Controller
builder.Services
    .AddControllers((options) =>
    {
        options.Filters.Add<ExceptionFilter>();
    });

// OpenAPI
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen((options) =>
    {
        options.AddSecurityDefinition("Bearer", new()
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new()
        {
            {
                new()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

// OpenTelemetry
builder.Services
    .AddOpenTelemetryMetrics((options) =>
    {
        options
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
            .AddPrometheusExporter()
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEventCountersInstrumentation((counters) => counters.AddEventSources(
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedData();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/Notification");

app.Run();
