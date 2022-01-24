#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["examples/microshop/MicroShop.Ordering/MicroShop.Ordering.csproj", "examples/microshop/MicroShop.Ordering/"]
RUN dotnet restore "examples/microshop/MicroShop.Ordering/MicroShop.Ordering.csproj"
COPY . .
WORKDIR "/src/examples/microshop/MicroShop.Ordering"
RUN dotnet build "MicroShop.Ordering.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroShop.Ordering.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroShop.Ordering.dll"]