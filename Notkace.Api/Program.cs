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
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<NotkaceContext>("notkace_db");

builder.Services.AddFastEndpoints();
builder.Services.RegisterServicesFromNotkaceApi();

var app = builder.Build();

// Dev only: apply pending EF migrations on startup so a fresh local database is schema-ready
// without a manual `dotnet ef database update`. In Azure the schema is a one-time static
// snapshot managed out-of-band, and the app's managed identity holds only db_datareader — so we
// skip Migrate() (it needs DDL rights) and keep least privilege. Schema changes there are applied
// manually with an admin login.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<NotkaceContext>();
    db.Database.Migrate();
}

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
