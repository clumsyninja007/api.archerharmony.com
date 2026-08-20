using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api;
using Notkace.Api.Data;

const string devCors = "devPolicy";
const string prodCors = "prodPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCors, policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader());
    options.AddPolicy(prodCors, policy =>
        policy.WithOrigins("https://notkace.hoelterling.me")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var connectionString = builder.Configuration.GetConnectionString("Notkace")
    ?? throw new InvalidOperationException("ConnectionStrings:Notkace is required");

builder.Services.AddDbContext<NotkaceContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<NotkaceContext>("notkace_db");

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromNotkaceApi();

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
