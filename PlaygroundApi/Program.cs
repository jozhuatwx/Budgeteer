global using PlaygroundApi.Data;
global using PlaygroundApi.Entities;
global using PlaygroundApi.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services
    .AddPooledDbContextFactory<PlaygroundContext>(options => options.UseInMemoryDatabase("Playground"))
    .AddScoped(provider => provider.GetRequiredService<IDbContextFactory<PlaygroundContext>>().CreateDbContext());

// Swagger
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", async (PlaygroundContext context) =>
{
    return await context.Users.GetAllAsync();
})
.WithOpenApi();

app.MapGet("/user/{id:int}", async (PlaygroundContext context, int id) =>
{
    return await context.Users.GetAsync(id);
})
.WithOpenApi();

app.MapPost("/user", async (PlaygroundContext context, string email, string name) =>
{
    var user = new User()
    {
        Email = email,
        Name = name
    };
    await context.Users.CreateAsync(user);
    await context.SaveChangesAsync();
})
.WithOpenApi();

app.MapPut("/user/{id:int}", async (PlaygroundContext context, int id, string name) =>
{
    var existingUser = await context.Users.GetAsync(id, true);

    if (existingUser != null)
    {
        existingUser.Name = name;

        context.Users.Update(existingUser);
        await context.SaveChangesAsync();
    }
})
.WithOpenApi();

app.MapDelete("/user/{id:int}", async (PlaygroundContext context, int id) =>
{
    var existingUser = await context.Users.GetAsync(id, true);

    if (existingUser != null)
    {
        context.Users.Delete(existingUser);
        await context.SaveChangesAsync();
    }
})
.WithOpenApi();

app.Run();
