FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

ARG ConnectionStrings__TelegramBot
ENV ConnectionStrings__TelegramBot $ConnectionStrings__TelegramBot

ARG ConnectionStrings__Notkace
ENV ConnectionStrings__Notkace $ConnectionStrings__Notkace

ARG TelegramBot__BotConfiguration__BotToken
ENV TelegramBot__BotConfiguration__BotToken $TelegramBot__BotConfiguration__BotToken

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
HEALTHCHECK CMD curl --fail http://localhost/health || exit
