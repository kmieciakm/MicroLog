#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["examples/microshop/MicroShop.Shipping/MicroShop.Shipping.csproj", "examples/microshop/MicroShop.Shipping/"]
RUN dotnet restore "examples/microshop/MicroShop.Shipping/MicroShop.Shipping.csproj"
COPY . .
WORKDIR "/src/examples/microshop/MicroShop.Shipping"
RUN dotnet build "MicroShop.Shipping.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroShop.Shipping.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroShop.Shipping.dll"]