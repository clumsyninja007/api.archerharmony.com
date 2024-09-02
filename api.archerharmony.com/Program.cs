using System;
using System.Net.Http;
using api.archerharmony.com;
using api.archerharmony.com.Extensions;
using api.archerharmony.com.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

var botToken = builder.GetSecretOrEnvVar("TelegramBot__BotConfiguration__BotToken");
if (string.IsNullOrEmpty(botToken))
{
    throw new Exception("TelegramBot__BotConfiguration__BotToken is required");
}

var botConfig = new BotConfiguration
{
    BotToken = botToken
};
builder.Services.AddSingleton(botConfig);

builder.Services.AddScoped<IUpdateService, UpdateService>();
builder.Services.AddSingleton<IBotService, BotService>();

builder.Services.AddHttpClient();

// builder.Services.AddHealthChecks()
    // .AddAsyncCheck("Http", async () =>
    // {
    //     using (var client = new HttpClient())
    //     {
    //         try
    //         {
    //             var response = await client.GetAsync("http://localhost/Users/owners");
    //             if (!response.IsSuccessStatusCode)
    //             {
    //                 throw new Exception("Notkace not responding with 200 OK");
    //             }
    //
    //             response = await client.GetAsync("http://localhost/update");
    //             if (!response.IsSuccessStatusCode)
    //             {
    //                 throw new Exception("Telegram Bot not responding with 200 OK");
    //             }
    //         }
    //         catch (Exception)
    //         {
    //             return await Task.FromResult(HealthCheckResult.Unhealthy());
    //         }
    //     }
    //     return await Task.FromResult(HealthCheckResult.Healthy());
    // });

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

// app.UseHealthChecks("/health");

app.Run();