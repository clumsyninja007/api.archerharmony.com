# Build from the repo root:  docker build -f Notkace.Api/Dockerfile -t notkace-api .
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Notkace.Api/Notkace.Api.csproj", "Notkace.Api/"]
RUN dotnet restore "Notkace.Api/Notkace.Api.csproj"

COPY . .
WORKDIR "/src/Notkace.Api"
RUN dotnet publish "Notkace.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notkace.Api.dll"]
