using System.Reflection;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    options.UseSqlServer(connectionString));

builder.Services.Configure<BotConfiguration>(config.GetSection("BotConfiguration"));

var botToken = config["BotConfiguration:BotToken"]
    ?? throw new InvalidOperationException("BotConfiguration:BotToken is required");
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddHttpClient();
builder.Services.AddScoped<ITelegramUpdateHandler, TelegramUpdateHandler>();

var app = builder.Build();

// Apply pending EF migrations once per host cold start (idempotent), so a fresh/updated
// TelegramBot database is schema-ready without a manual `dotnet ef database update`.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();
    db.Database.Migrate();
}

app.Run();
