FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroLog.Collector/MicroLog.Collector.csproj", "MicroLog.Collector/"]
COPY ["MicroLog.Registry.MongoDb/MicroLog.Sink.MongoDb.csproj", "MicroLog.Registry.MongoDb/"]
COPY ["MicroLog.Core/MicroLog.Core.csproj", "MicroLog.Core/"]
COPY ["MicroLog.Driver.RabbitMq/MicroLog.Collector.RabbitMq.csproj", "MicroLog.Driver.RabbitMq/"]
RUN dotnet restore "MicroLog.Collector/MicroLog.Collector.csproj"
COPY . .
WORKDIR "/src/MicroLog.Collector"
RUN dotnet build "MicroLog.Collector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroLog.Collector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroLog.Collector.dll"]
