# Archer Harmony API

A multi-tenant .NET 9 ASP.NET Core Web API serving multiple domains and products using FastEndpoints and a feature-based architecture.

## Overview

This API serves as the backend for multiple applications:
- **Hoelterling**: Personal portfolio/resume API
- **Notkace**: Ticketing system API
- **TelegramBot**: Telegram bot integration endpoints

## Technology Stack

- **.NET 9** - ASP.NET Core Web API
- **FastEndpoints** - Minimal API routing framework
- **Entity Framework Core** - ORM for TelegramBot and Notkace databases
- **Dapper** - Micro-ORM for complex queries (Hoelterling)
- **DbUp** - Database migration tool for Hoelterling database
- **MySQL/MariaDB** - Three separate databases for each domain
- **Docker** - Containerized deployment
- **GitHub Actions** - CI/CD pipeline

## Project Structure

```
api.archerharmony.com/
├── api.archerharmony.com/          # Main Web API project
│   ├── Features/                   # Feature-based endpoint organization
│   │   ├── Hoelterling/           # Portfolio API endpoints
│   │   ├── Notkace/               # Ticketing system endpoints
│   │   └── TelegramBot/           # Telegram bot endpoints
│   ├── Extensions/                # Extension methods and utilities
│   └── Program.cs                 # Application entry point
└── api.archerharmony.com.Data/    # Data access layer
    ├── Entities/                  # EF Core entities and contexts
    └── Migrations/                # Database migration scripts
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- MySQL/MariaDB server (or Docker for databases)
- Docker (optional, for containerized deployment)

### Configuration

The application requires three connection strings configured via environment variables or user secrets:

```bash
ConnectionStrings__Hoelterling="Server=localhost;Database=hoelterling;User=root;Password=xxx"
ConnectionStrings__TelegramBot="Server=localhost;Database=telegrambot;User=root;Password=xxx"
ConnectionStrings__Notkace="Server=localhost;Database=notkace;User=root;Password=xxx"
```

**Development**: Use .NET user secrets
```bash
dotnet user-secrets set "ConnectionStrings__Hoelterling" "your-connection-string"
dotnet user-secrets set "ConnectionStrings__TelegramBot" "your-connection-string"
dotnet user-secrets set "ConnectionStrings__Notkace" "your-connection-string"
```

**Production**: Use environment variables or Docker secrets (file paths)

### Build and Run

```bash
# Build the solution
dotnet build

# Run the API locally
dotnet run --project api.archerharmony.com/api.archerharmony.com.csproj

# Publish for production
dotnet publish -c Release -o out
```

The API will be available at `http://localhost:5000` (or configured port).

### Docker

```bash
# Build Docker image
docker build -t api.archerharmony.com:latest .

# Run with Docker Compose
docker compose up -d api
```

## Architecture

### Feature-Based Organization

Endpoints are organized by domain and feature, promoting maintainability and discoverability:

```
Features/
├── Hoelterling/
│   ├── HoelterlingGroup.cs         # Route group (/hoelterling)
│   ├── GetWorkExperience/
│   │   ├── Endpoint.cs             # HTTP endpoint handler
│   │   ├── Data.cs                 # Data access logic
│   │   ├── Request.cs              # Request DTO
│   │   └── WorkExperience.cs       # Response DTO
│   └── GetSkills/
│       └── ...
```

### FastEndpoints Pattern

Each feature follows a consistent structure:

**Endpoint.cs** - HTTP handling
```csharp
public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("work-experience/{id}");
        Group<HoelterlingGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await data.GetWorkExperience(req.Id);
        await SendOkAsync(result, ct);
    }
}
```

**Data.cs** - Data access
```csharp
[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory factory) : IData
{
    public async Task<WorkExperience> GetWorkExperience(int id)
    {
        // Dapper or EF Core data access
    }
}
```

Services are automatically registered using FastEndpoints source generators via the `[RegisterService]` attribute.

### Data Access Strategy

- **DbUp**: SQL migration scripts for Hoelterling database (run at startup)
- **Dapper**: Complex queries with multiple result sets
- **EF Core**: TelegramBot and Notkace databases (code-first migrations)
- **Database Connection Factory**: Typed access to multiple databases

## Database Migrations

### Hoelterling (DbUp)

Create SQL migration scripts:

1. Add `.sql` file to `api.archerharmony.com.Data/Migrations/Hoelterling/`
2. Name with sequential numbering: `Script####_Description.sql`
3. Migrations run automatically on application startup

### TelegramBot / Notkace (EF Core)

```bash
# Add migration for TelegramBot context
dotnet ef migrations add MigrationName --context TelegramBotContext --project api.archerharmony.com.Data

# Add migration for Notkace context
dotnet ef migrations add MigrationName --context NotkaceContext --project api.archerharmony.com.Data

# Apply migrations (or let application startup handle it)
dotnet ef database update --context TelegramBotContext
```

## API Endpoints

### Health Check
- `GET /healthz` - Health check with database connectivity verification

### Hoelterling (Portfolio)
- Base path: `/hoelterling`
- Anonymous access enabled

### Notkace (Ticketing)
- Base path: `/notkace`
- (Authentication/authorization as configured per endpoint)

### TelegramBot
- Base path: `/telegram`
- (Authentication/authorization as configured per endpoint)

## CORS Configuration

**Development**: Allows localhost origins (ports 8080, 5173)
- `http://localhost:8080`
- `http://localhost:5173`
- `http://127.0.0.1:8080`

**Production**: Allows specific domains
- `https://notkace.archerharmony.com`
- `https://archer.hoelterling.me`

## Deployment

Automatic deployment via GitHub Actions on push to `main` branch:

1. Builds Docker image
2. Deploys to DigitalOcean droplet via SSH
3. Runs with Docker Compose

See `.github/workflows/deploy.yml` for details.

## Development Guidelines

### Adding a New Endpoint

1. Create feature folder under appropriate domain in `Features/[Domain]/[FeatureName]/`
2. Add `Endpoint.cs`, `Data.cs`, `Request.cs`, and response DTOs
3. Implement `IData` interface with `[RegisterService]` attribute
4. Associate endpoint with route group using `Group<DomainGroup>()`
5. Services are automatically registered by FastEndpoints

### Secrets Management

Use `GetSecretOrEnvVar(builder, "Key")` for sensitive configuration:
- Supports double-underscore notation: `ConnectionStrings__Hoelterling`
- Reads file content if value is a file path (for Docker secrets)
- Falls back to standard configuration with colon notation

## License

(Add your license information here)

## Contact

(Add your contact information here)