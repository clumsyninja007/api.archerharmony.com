using FastEndpoints;
using Hoelterling.Api;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

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
      return client.GetContainer("HoelterlingDb", "portfolio");
  });

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromHoelterlingApi();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();