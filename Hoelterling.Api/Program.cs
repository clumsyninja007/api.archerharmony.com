using Azure.Identity;
using FastEndpoints;
using Hoelterling.Api;
using Hoelterling.Api.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Web;

const string devCors = "devPolicy";
const string prodCors = "prodPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCors, policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
    options.AddPolicy(prodCors, policy =>
        policy.WithOrigins("https://archer.hoelterling.me")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var databaseId = builder.Environment.IsDevelopment() ? "HoelterlingDb-Test" : "HoelterlingDb";
builder.Services.AddSingleton(_ =>
  {
      var options = new CosmosClientOptions
      {
          SerializerOptions = new CosmosSerializationOptions
          {
              PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
          }
      };

      // Local dev uses the account key from user-secrets (ConnectionStrings:Cosmos). In Azure
      // no key is configured, so authenticate with the Container App's managed identity via
      // Cosmos:Endpoint (the account URI) + DefaultAzureCredential — no secret anywhere.
      var connectionString = builder.Configuration.GetConnectionString("Cosmos");
      var client = string.IsNullOrEmpty(connectionString)
          ? new CosmosClient(
              builder.Configuration["Cosmos:Endpoint"]
                  ?? throw new InvalidOperationException(
                      "Set ConnectionStrings:Cosmos (local) or Cosmos:Endpoint (managed identity)."),
              new DefaultAzureCredential(),
              options)
          : new CosmosClient(connectionString, options);

      return client.GetContainer(databaseId, "portfolio");
  });

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ContentAdmin", policy => policy.RequireRole("content-admin"));
});

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromHoelterlingApi();

builder.Services.AddHealthChecks()
    .AddCheck<CosmosHealthCheck>("cosmos");

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

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.MapHealthChecks("/healthz");

app.Run();