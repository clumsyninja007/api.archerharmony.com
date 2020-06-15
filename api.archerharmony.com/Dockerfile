#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

ENV ConnectionStrings__TelegramBot=
ENV ConnectionStrings__Notkace=
ENV TelegramBot__BotConfiguration__BotToken=

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["api.archerharmony.com/api.archerharmony.com.csproj", "api.archerharmony.com/"]
RUN dotnet restore "api.archerharmony.com/api.archerharmony.com.csproj"
COPY . .
WORKDIR "/src/api.archerharmony.com"
RUN dotnet build "api.archerharmony.com.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.archerharmony.com.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.archerharmony.com.dll"]
HEALTHCHECK CMD curl --fail http://localhost/health || exit