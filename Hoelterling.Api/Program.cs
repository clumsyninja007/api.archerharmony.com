using FastEndpoints;
using Hoelterling.Api;
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ContentAdmin", policy => policy.RequireRole("content-admin"));
});

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromHoelterlingApi();

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

app.Run();