using System;
using System.Net.Http;
using System.Threading.Tasks;
using api.archerharmony.com;
using api.archerharmony.com.DbContext;
using api.archerharmony.com.Extensions;
using api.archerharmony.com.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://127.0.0.1:8080",
                            "http://localhost:8080",
                            "https://notkace.archerharmony.com")
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddDbContext<TelegramBotContext>(options =>
    options.UseMySql(builder.GetSecretOrEnvVar("ConnectionStrings__TelegramBot"),
        mySqlOptions => { mySqlOptions.ServerVersion(new Version(10, 4, 12), ServerType.MariaDb); }));

builder.Services.AddDbContext<NotkaceContext>(options =>
    options.UseMySql(builder.GetSecretOrEnvVar("ConnectionStrings__Notkace"),
        mySqlOptions => { mySqlOptions.ServerVersion(new Version(10, 4, 12), ServerType.MariaDb); }));

builder.Services.AddScoped<IUpdateService, UpdateService>();
builder.Services.AddSingleton<IBotService, BotService>();

var botConfig = new BotConfiguration
{
    BotToken = builder.GetSecretOrEnvVar("TelegramBot__BotConfiguration__BotToken")
};
builder.Services.AddSingleton(botConfig);

builder.Services.AddHttpClient();

builder.Services.AddHealthChecks()
    .AddAsyncCheck("Http", async () =>
    {
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync("http://localhost/Users/owners");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Notkace not responding with 200 OK");
                }

                response = await client.GetAsync("http://localhost/update");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Telegram Bot not responding with 200 OK");
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
        return await Task.FromResult(HealthCheckResult.Healthy());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireCors(myAllowSpecificOrigins);
});

app.UseHealthChecks("/health");

app.Run();