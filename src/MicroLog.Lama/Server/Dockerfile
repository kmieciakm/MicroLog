FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroLog.Lama/Server/MircoLog.Lama.Server.csproj", "MicroLog.Lama/Server/"]
COPY ["MicroLog.Lama/Client/MircoLog.Lama.Client.csproj", "MicroLog.Lama/Client/"]
COPY ["MicroLog.Lama/Shared/MircoLog.Lama.Shared.csproj", "MicroLog.Lama/Shared/"]
RUN dotnet restore "MicroLog.Lama/Server/MircoLog.Lama.Server.csproj"
COPY . .
WORKDIR "/src/MicroLog.Lama/Server"
RUN dotnet build "MircoLog.Lama.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MircoLog.Lama.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MircoLog.Lama.Server.dll"]