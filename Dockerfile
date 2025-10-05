FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Install curl and clean up in same layer to reduce image size
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["api.archerharmony.com/api.archerharmony.com.csproj", "api.archerharmony.com/"]
RUN dotnet restore "api.archerharmony.com/api.archerharmony.com.csproj"

COPY . .
WORKDIR "/src/api.archerharmony.com"
RUN dotnet build "api.archerharmony.com.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.archerharmony.com.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.archerharmony.com.dll"]
HEALTHCHECK CMD curl --fail http://localhost:8080/healthz || exit
