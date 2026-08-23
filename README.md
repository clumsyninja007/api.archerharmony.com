# Archer Harmony API

A .NET 10 solution of three independently deployed backends, each serving one product and owning its own datastore and hosting model.

## Overview

| Project | Product | Host | Datastore | Auth |
|---|---|---|---|---|
| **Hoelterling.Api** | Personal portfolio / resume API | Azure Container Apps | Azure Cosmos DB (NoSQL) | Entra ID (JWT) |
| **Notkace.Api** | Ticketing system API | Azure Container Apps | Azure SQL (EF Core) | Anonymous (per-endpoint) |
| **TelegramBot** | Telegram bot (webhook + scheduler) | Azure Functions | Azure SQL (EF Core) | Shared-secret path token |

The projects share no code and no database. Each has its own `Dockerfile`/host, `UserSecretsId`, and GitHub Actions workflow, and each deploys on its own when its folder changes. The Vue SPAs that consume these APIs (`archer.hoelterling.me`, `notkace.hoelterling.me`) live in separate frontend repos and are hosted on Azure Static Web Apps.

## Technology Stack

- **.NET 10** — ASP.NET Core (the two APIs) and the Azure Functions isolated worker (TelegramBot)
- **FastEndpoints 8.3** — endpoint routing for both APIs
- **Azure Cosmos DB** (`Microsoft.Azure.Cosmos`) — Hoelterling data
- **Entity Framework Core 10 (SQL Server)** — Notkace and TelegramBot data on Azure SQL
- **Microsoft.Identity.Web** — Entra ID JWT validation (Hoelterling)
- **Azure.Identity** — `DefaultAzureCredential` / managed identity for keyless data access in Azure
- **Telegram.Bot** — Telegram client (TelegramBot)
- **Docker + GHCR** — container images for the two APIs
- **GitHub Actions** — per-project CI/CD to Azure (OIDC login, no stored Azure credentials)

## Solution Layout

```
api.archerharmony.com/
├── Hoelterling.Api/      # Portfolio API  → Azure Container Apps (ca-hoelterling-api)
│   ├── UseCases/         #   one folder per operation (Endpoint / Data / Request / Response, + Dtos)
│   ├── Extensions/       #   Cosmos query helper, claims + request helpers
│   ├── HealthChecks/     #   CosmosHealthCheck
│   ├── Dockerfile
│   └── Program.cs
├── Notkace.Api/          # Ticketing API  → Azure Container Apps (ca-notkace-api)
│   ├── UseCases/         #   endpoints grouped by resource (Tickets / Assets / Users)
│   ├── Data/             #   NotkaceContext, entities, EF Core migrations
│   ├── Dockerfile
│   └── Program.cs
├── TelegramBot/          # Telegram bot  → Azure Functions
│   ├── Functions/        #   UpdateFunction (HTTP webhook), WaterReminderFunction (timer)
│   ├── Services/         #   TelegramUpdateHandler — command logic (host-agnostic)
│   ├── Data/             #   TelegramBotContext, entities, EF Core migrations
│   ├── host.json
│   └── Program.cs
└── .github/workflows/    # deploy-hoelterling-api.yml, deploy-notkace-api.yml, deploy-telegrambot.yml
```

## Architecture

### FastEndpoints (Hoelterling.Api, Notkace.Api)

Each operation lives in its own `UseCases/<Area>/<Operation>/` folder with an `Endpoint`, a `Data` class, and request/response DTOs. `Data` classes are auto-registered by the FastEndpoints source generator via `[RegisterService<IData>(LifeTime.Scoped)]` and picked up by `RegisterServicesFrom<Project>()` in `Program.cs`.

```csharp
public class Endpoint(IData data) : Endpoint<Request, IEnumerable<WorkExperience>>
{
    public override void Configure()
    {
        Get("person/{personId}/experience");
        AllowAnonymous();               // or Roles("content-admin") / Group<TicketsGroup>()
    }

    public override async Task HandleAsync(Request req, CancellationToken ct) { ... }
}
```

- **Hoelterling.Api** sets access per endpoint: public reads use `AllowAnonymous()`; admin writes use `Roles("content-admin")`.
- **Notkace.Api** uses route groups (`TicketsGroup` → `hdTickets`, plus `AssetsGroup`, `UsersGroup`), each currently `AllowAnonymous()`.

### Data access

- **Hoelterling.Api** injects a Cosmos `Container` (registered as a singleton in `Program.cs`) and runs SQL-style queries through the `QueryAsync` extension in `Extensions/CosmosContainerExtensions.cs`. Documents are typed (`*Document` DTOs) and localized via `LocalizationHelper`.
- **Notkace.Api** uses `NotkaceContext` (EF Core) with `EnableRetryOnFailure()`. In Azure the app's managed identity holds only `db_datareader`; schema is managed out-of-band, so `Database.Migrate()` runs **only in Development**.
- **TelegramBot** uses `TelegramBotContext` (EF Core) and migrates on cold start (guarded — a failure logs and lets the host keep running).

### Authentication

- **Hoelterling.Api** validates Entra ID JWTs via `AddMicrosoftIdentityWebApi(AzureAd)`. Admin endpoints require the `content-admin` role (`ContentAdmin` policy / `Roles("content-admin")`).
- **Notkace.Api** endpoints are anonymous.
- **TelegramBot** webhook authenticates by requiring the `{token}` path segment to equal the bot token (the webhook URL is the shared secret).

### Keyless access in Azure

No connection secrets are stored in Azure. Each service authenticates to its datastore with its **system-assigned managed identity** via `DefaultAzureCredential`:

- Hoelterling → Cosmos: local dev uses `ConnectionStrings:Cosmos` (account key from user-secrets, database `HoelterlingDb-Test`); Azure uses `Cosmos:Endpoint` (account URI) + managed identity (database `HoelterlingDb`, container `portfolio`).
- Notkace / TelegramBot → Azure SQL: connection string uses `Authentication=Active Directory Managed Identity`; the identity is a contained DB user with least-privilege rights.

## Getting Started

### Prerequisites

- .NET 10 SDK
- Azure Functions Core Tools v4 (to run TelegramBot locally)
- Access to a Cosmos DB account and a SQL Server / Azure SQL database (or local equivalents)

### Configuration (local)

Each project reads secrets from .NET user secrets (its own `UserSecretsId`).

**Hoelterling.Api**
```bash
dotnet user-secrets set "ConnectionStrings:Cosmos" "<cosmos-account-connection-string>" \
  --project Hoelterling.Api
# AzureAd section (TenantId, ClientId, etc.) also via user-secrets or appsettings.Development.json
```

**Notkace.Api**
```bash
dotnet user-secrets set "ConnectionStrings:Notkace" "<sql-connection-string>" \
  --project Notkace.Api
```

**TelegramBot** (`local.settings.json` or user-secrets; see `local.settings.example.json`)
```bash
dotnet user-secrets set "ConnectionStrings:TelegramBot" "<sql-connection-string>" --project TelegramBot
dotnet user-secrets set "BotConfiguration:BotToken"     "<telegram-bot-token>"    --project TelegramBot
```

In Azure these are supplied as Container App / Function App application settings, and datastore access uses managed identity (see above).

### Build and Run

```bash
# Build everything
dotnet build

# Run an API locally
dotnet run --project Hoelterling.Api
dotnet run --project Notkace.Api

# Run the Functions app locally (requires Core Tools)
cd TelegramBot && func start
```

### Docker (the two APIs)

Images build from the repo root so the build context includes the project:

```bash
docker build -f Hoelterling.Api/Dockerfile -t hoelterling-api .
docker build -f Notkace.Api/Dockerfile     -t notkace-api .
```

## Health Checks

Both APIs expose `GET /healthz`:
- **Hoelterling.Api** — `CosmosHealthCheck` reads the container to verify connectivity.
- **Notkace.Api** — `AddDbContextCheck<NotkaceContext>` verifies the database is reachable.

## Database Migrations (EF Core)

```bash
# Notkace
dotnet ef migrations add <Name> --project Notkace.Api --context NotkaceContext

# TelegramBot
dotnet ef migrations add <Name> --project TelegramBot --context TelegramBotContext
```

Notkace applies migrations automatically **only in Development** (least-privilege identity in Azure); apply schema changes there with an admin login. TelegramBot applies pending migrations on host cold start. Cosmos (Hoelterling) is schema-less and needs no migrations.

## CORS

| API | Development | Production |
|---|---|---|
| Hoelterling.Api | `http://localhost:5173` | `https://archer.hoelterling.me` |
| Notkace.Api | `http://localhost:5173`, `http://localhost:8080` | `https://notkace.hoelterling.me` |

## Deployment

Deployment is per-project via GitHub Actions on push to `main`, path-filtered so only the changed project deploys. All Azure logins use OIDC (federated credentials) — no Azure secrets stored in GitHub.

- **`deploy-hoelterling-api.yml`** / **`deploy-notkace-api.yml`** — build a Docker image, push to GHCR (`ghcr.io/<owner>/{hoelterling,notkace}-api`), then roll the Container App with `az containerapp update`.
- **`deploy-telegrambot.yml`** — `dotnet publish` and deploy to the Azure Function App via `Azure/functions-action` (publish-profile auth).

### Required GitHub configuration

- Secrets: `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID` (OIDC), `TELEGRAM_FUNCTIONAPP_PUBLISH_PROFILE`
- Variables: `TELEGRAM_FUNCTIONAPP_NAME`
- `GITHUB_TOKEN` (built-in) pushes images to GHCR.

## Adding an Endpoint (APIs)

1. Create `UseCases/<Area>/<Operation>/` with `Endpoint.cs`, `Data.cs` (+ `Request`/`Response`, and `Dtos` as needed).
2. Implement the `IData` interface and annotate the class with `[RegisterService<IData>(LifeTime.Scoped)]`.
3. In `Configure()`, set the route and access (`AllowAnonymous()`, `Roles(...)`, or `Group<...>()`).
