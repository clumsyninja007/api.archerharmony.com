using FastEndpoints;
using Hoelterling.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

var databaseId = builder.Environment.IsDevelopment() ? "HoelterlingDb-Test" : "HoelterlingDb";
builder.Services.AddSingleton(_ =>
  {
      var client = new CosmosClient(
          builder.Configuration.GetConnectionString("Cosmos"),
          new CosmosClientOptions
          {
              SerializerOptions = new CosmosSerializationOptions
              {
                  PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
              }
          });
      return client.GetContainer(databaseId, "portfolio");
  });

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// ⚠️ Entra puts app roles in the "roles" claim; ASP.NET's default role claim type is different,
// so RequireRole / FastEndpoints Roles(...) won't see it unless we point it at "roles".
builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options => options.TokenValidationParameters.RoleClaimType = "roles");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ContentAdmin", policy => policy.RequireRole("content-admin"));
});

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromHoelterlingApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();