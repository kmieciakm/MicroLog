FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroLog.Collector/MicroLog.Collector.csproj", "MicroLog.Collector/"]
COPY ["MicroLog.Collector.RabbitMq/MicroLog.Collector.RabbitMq.csproj", "MicroLog.Collector.RabbitMq/"]
COPY ["MicroLog.Core/MicroLog.Core.csproj", "MicroLog.Core/"]
COPY ["MicroLog.Sink.MongoDb/MicroLog.Sink.MongoDb.csproj", "MicroLog.Sink.MongoDb/"]
COPY ["MicroLog.Sink.Hub/MicroLog.Sink.Hub.csproj", "MicroLog.Sink.Hub/"]
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