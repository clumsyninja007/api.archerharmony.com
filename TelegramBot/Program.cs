using System.Reflection;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Data;
using TelegramBot.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// The isolated worker doesn't load user-secrets by default (unlike ASP.NET). Add it so local
// dev can keep the connection string + bot token out of local.settings.json. No-op in Azure
// (no secrets file), where app settings supply these via environment variables.
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);

var config = builder.Configuration;

var connectionString = config.GetConnectionString("TelegramBot")
    ?? throw new InvalidOperationException("ConnectionStrings:TelegramBot is required");

builder.Services.AddDbContext<TelegramBotContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

builder.Services.Configure<BotConfiguration>(config.GetSection("BotConfiguration"));

var botToken = config["BotConfiguration:BotToken"]
    ?? throw new InvalidOperationException("BotConfiguration:BotToken is required");
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddHttpClient();
builder.Services.AddScoped<ITelegramUpdateHandler, TelegramUpdateHandler>();

var app = builder.Build();

// Apply pending EF migrations once per host cold start (idempotent). Guarded so an
// unreachable database (paused, firewall, transient) logs and continues rather than crashing
// the whole host — data-backed functions will error until the database is reachable, but the
// host stays up and the next cold start retries.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        services.GetRequiredService<TelegramBotContext>().Database.Migrate();
    }
    catch (Exception ex)
    {
        services.GetRequiredService<ILogger<Program>>()
            .LogError(ex, "Startup database migration failed; host will continue.");
    }
}

app.Run();
