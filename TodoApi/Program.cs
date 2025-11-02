using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Build the Postgres connection string from env (docker) or appsettings fallback
string GetConnString()
{
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var db   = Environment.GetEnvironmentVariable("POSTGRES_DB");
    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    if (!string.IsNullOrWhiteSpace(host) &&
        !string.IsNullOrWhiteSpace(db) &&
        !string.IsNullOrWhiteSpace(user) &&
        !string.IsNullOrWhiteSpace(pass))
    {
        return $"Host={host};Port=5432;Database={db};Username={user};Password={pass}";
    }
    // fallback to appsettings.json
    return builder.Configuration.GetConnectionString("Postgres")
           ?? "Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=postgres";
}

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseNpgsql(GetConnString()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});

var app = builder.Build();

// Apply migrations / create schema on startup (simple & safe for dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.Migrate(); // uses migrations if present; creates database if not
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1"));
}

app.MapControllers();
app.Run();
