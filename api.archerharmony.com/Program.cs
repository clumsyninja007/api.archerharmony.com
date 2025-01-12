using api.archerharmony.com.Entities.Context;
using api.archerharmony.com.Extensions;
using api.archerharmony.com.Features.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

const string devCors = "devPolicy";
const string prodCors = "prodPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCors, policy =>
        policy.WithOrigins("http://127.0.0.1:8080", "http://localhost:8080")
            .AllowAnyMethod());
    options.AddPolicy(prodCors, policy =>
        policy.WithOrigins("https://notkace.archerharmony.com")
            .AllowAnyMethod());
});

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database_health_check");

var telegramBotConnString = builder.GetSecretOrEnvVar("ConnectionStrings__TelegramBot");
if (string.IsNullOrEmpty(telegramBotConnString))
{
    throw new Exception("ConnectionStrings__TelegramBot is required");
}

builder.Services.AddDbContext<TelegramBotContext>(options =>
    options.UseMySql(
        telegramBotConnString,
        new MariaDbServerVersion(new Version(10, 4, 12)))
    );

var notkaceConnString = builder.GetSecretOrEnvVar("ConnectionStrings__Notkace");
if (string.IsNullOrEmpty(notkaceConnString))
{
    throw new Exception("ConnectionStrings__Notkace is required");
}

builder.Services.AddDbContext<NotkaceContext>(options =>
    options.UseMySql(notkaceConnString,
        new MariaDbServerVersion(new Version(10, 4, 12)))
    );

builder.AddTelegramBotClient();

builder.Services.AddHttpClient();

builder.Services.AddFastEndpoints();

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