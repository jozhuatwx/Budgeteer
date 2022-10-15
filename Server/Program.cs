using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
    // Background
    .AddHostedService<BackgroundWorkerService>()
    .AddSingleton<BackgroundQueueService>();

// Controller
builder.Services
    .AddControllers();

// Swagger
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

app.Run();
