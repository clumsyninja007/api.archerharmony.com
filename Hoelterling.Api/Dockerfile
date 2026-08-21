# Build from the repo root:  docker build -f Hoelterling.Api/Dockerfile -t hoelterling-api .
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Hoelterling.Api/Hoelterling.Api.csproj", "Hoelterling.Api/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Hoelterling.Api/Hoelterling.Api.csproj"

COPY . .
WORKDIR "/src/Hoelterling.Api"
RUN dotnet publish "Hoelterling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hoelterling.Api.dll"]
