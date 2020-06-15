using api.archerharmony.com.DbContext;
using api.archerharmony.com.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace api.archerharmony.com
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:8080",
                                        "http://localhost:8080",
                                        "https://notkace.archerharmony.com")
                        .AllowAnyMethod();
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddDbContext<TelegramBotContext>(options =>
                options.UseMySql(Configuration["ConnectionStrings:TelegramBot"],
                    mySqlOptions => { mySqlOptions.ServerVersion(new Version(10, 4, 12), ServerType.MariaDb); }));

            services.AddDbContext<NotkaceContext>(options =>
                options.UseMySql(Configuration["ConnectionStrings:Notkace"],
                    mySqlOptions => { mySqlOptions.ServerVersion(new Version(10, 4, 12), ServerType.MariaDb); }));

            services.AddScoped<IUpdateService, UpdateService>();
            services.AddSingleton<IBotService, BotService>();

            services.Configure<BotConfiguration>(Configuration.GetSection("TelegramBot:BotConfiguration"));

            services.AddHttpClient();

            services.AddHealthChecks()
                .AddAsyncCheck("Http", async () =>
                { 
                    using (HttpClient client = new HttpClient())
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
            });

            app.UseHealthChecks("/health");
        }
    }
}
