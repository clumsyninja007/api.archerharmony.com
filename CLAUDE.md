# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 9 ASP.NET Core Web API project using FastEndpoints for routing and minimal API architecture. The API serves multiple domains/products:
- **Hoelterling**: Personal portfolio/resume API
- **Notkace**: Ticketing system API
- **TelegramBot**: Telegram bot integration endpoints

## Project Structure

The solution contains two projects:
- **api.archerharmony.com**: Main Web API project with endpoints and business logic
- **api.archerharmony.com.Data**: Data access layer with EF Core contexts, entities, and migrations

## Build and Run Commands

```bash
# Build the solution
dotnet build

# Run the API locally
dotnet run --project api.archerharmony.com/api.archerharmony.com.csproj

# Publish for production
dotnet publish -c Release -o out

# Build Docker image
docker build -t api.archerharmony.com:latest .

# Run with Docker (requires docker-compose setup)
docker compose up -d api
```

## Database Configuration

The application uses **DbUp** for database migrations and connects to three separate MySQL/MariaDB databases:
- `Hoelterling`: Portfolio/resume data
- `TelegramBot`: Bot-related data (uses EF Core context)
- `Notkace`: Ticketing system data (uses EF Core context)

Connection strings are loaded via `GetSecretOrEnvVar()` which supports:
1. Environment variables (e.g., `ConnectionStrings__Hoelterling`)
2. User secrets (development)
3. File paths (for Docker secrets)

### Adding Database Migrations

**For Hoelterling database** (DbUp):
1. Create a new `.sql` file in `api.archerharmony.com.Data/Migrations/Hoelterling/`
2. Name it with sequential numbering: `Script####_Description.sql`
3. Ensure it's marked as an embedded resource (already configured in `.csproj`)
4. DbUp automatically runs migrations on application startup

**For other databases** (EF Core):
```bash
# Add migration for TelegramBot context
dotnet ef migrations add MigrationName --context TelegramBotContext --project api.archerharmony.com.Data

# Add migration for Notkace context
dotnet ef migrations add MigrationName --context NotkaceContext --project api.archerharmony.com.Data
```

## Architecture Patterns

### Feature-Based Organization

Endpoints are organized by feature domain under `Features/[Domain]/[Feature]/`:
```
Features/
├── Hoelterling/
│   ├── HoelterlingGroup.cs         # Route group configuration
│   ├── GetWorkExperience/
│   │   ├── Endpoint.cs             # FastEndpoints endpoint
│   │   ├── Data.cs                 # Data access (Dapper queries)
│   │   ├── Request.cs              # Request DTO
│   │   └── WorkExperience.cs       # Response DTO
│   └── GetSkills/
│       └── ...
├── Notkace/
└── TelegramBot/
```

### FastEndpoints Pattern

Each feature endpoint follows this structure:

**Endpoint.cs**: Defines routing and HTTP handling
```csharp
public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("path/{param}");
        Group<FeatureGroup>();  // Associates with route group
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await data.GetData(req.Param);
        await Send.OkAsync(result, ct);
    }
}
```

**Data.cs**: Contains data access logic
```csharp
[RegisterService<IData>(LifeTime.Scoped)]  // Auto-registered by FastEndpoints
public class Data(IDatabaseConnectionFactory factory) : IData
{
    // Use Dapper for complex queries or EF Core contexts for simpler CRUD
}
```

**Route Groups**: Each domain has a Group class that configures base path and policies:
```csharp
public sealed class HoelterlingGroup : Group
{
    public HoelterlingGroup()
    {
        Configure("hoelterling", ep => { ep.AllowAnonymous(); });
    }
}
```

### Data Access Strategy

- **Dapper**: Used for complex queries with multiple result sets (see `GetWorkExperience/Data.cs`)
- **EF Core**: Used for `TelegramBot` and `Notkace` databases via DbContexts
- **Database Connection Factory**: Provides typed access to different databases via `IDatabaseConnectionFactory` and `DatabaseType` enum

### Dependency Registration

FastEndpoints uses source generators for service registration:
- Use `[RegisterService<IInterface>(LifeTime.Scoped)]` attribute on implementation classes
- Services are automatically registered via `RegisterServicesFromapiarcherharmonycom()`

## Secrets Management

Use `GetSecretOrEnvVar(builder, "Key")` for all sensitive configuration:
- Supports double-underscore notation (e.g., `ConnectionStrings__Hoelterling`)
- Automatically converts to colon notation for configuration lookup
- Reads file content if environment variable points to a file path

## CORS Configuration

Two CORS policies configured in `Program.cs`:
- **devPolicy**: Allows localhost origins (8080, 5173)
- **prodPolicy**: Allows `notkace.archerharmony.com` and `archer.hoelterling.me`

## Deployment

Automatic deployment via GitHub Actions (`.github/workflows/deploy.yml`):
- Triggers on push to `main` branch
- Builds Docker image and deploys to DigitalOcean droplet
- Uses SSH for deployment with Docker Compose

Health check endpoint available at `/healthz` (includes database connectivity check).