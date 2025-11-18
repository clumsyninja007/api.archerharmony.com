using api.archerharmony.com;
using api.archerharmony.com.Entities.Context;
using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;
using api.archerharmony.com.Features.Health;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

const string devCors = "devPolicy";
const string prodCors = "prodPolicy";

var mariaDbVersion = new Version(12, 0, 2);

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCors, policy =>
        policy.WithOrigins("http://127.0.0.1:8080", "http://localhost:8080", "http://localhost:5173")
            .AllowAnyMethod());
    options.AddPolicy(prodCors, policy =>
        policy.WithOrigins("https://notkace.archerharmony.com", "https://archer.hoelterling.me")
            .AllowAnyMethod());
});

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database_health_check");

var connectionString = builder.GetSecretOrEnvVar("ConnectionStrings__Hoelterling");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("ConnectionStrings__Hoelterling is required");
}
EnsureDatabase.For.MySqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .MySqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(typeof(DatabaseType).Assembly)
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    throw new Exception("Failed to upgrade database", result.Error);
}

var telegramBotConnString = builder.GetSecretOrEnvVar("ConnectionStrings__TelegramBot");
if (string.IsNullOrEmpty(telegramBotConnString))
{
    throw new Exception("ConnectionStrings__TelegramBot is required");
}

builder.Services.AddDbContext<TelegramBotContext>(options =>
    options.UseMySql(
        telegramBotConnString,
        new MariaDbServerVersion(mariaDbVersion))
    );

var notkaceConnString = builder.GetSecretOrEnvVar("ConnectionStrings__Notkace");
if (string.IsNullOrEmpty(notkaceConnString))
{
    throw new Exception("ConnectionStrings__Notkace is required");
}

builder.Services.AddDbContext<NotkaceContext>(options =>
    options.UseMySql(notkaceConnString,
        new MariaDbServerVersion(mariaDbVersion))
    );

builder.Services.AddSingleton<IDatabaseConnectionFactory>(new DatabaseConnectionFactory(
    new Dictionary<DatabaseType, string>
    {
        { DatabaseType.Hoelterling, connectionString },
        { DatabaseType.TelegramBot, telegramBotConnString },
        { DatabaseType.Notkace, notkaceConnString }
    }));

builder.AddTelegramBotClient();

builder.Services.AddHttpClient()
    .AddFastEndpoints()
    .RegisterServicesFromapiarcherharmonycom(); // FE source generated services

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(devCors);
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseCors(prodCors);
    app.UseHsts();
}

app.UseFastEndpoints();

app.MapHealthChecks("/healthz");

app.Run();