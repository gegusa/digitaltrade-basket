FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DigitalTrade.Basket/DigitalTrade.Basket.csproj", "DigitalTrade.Basket/"]
RUN dotnet restore "DigitalTrade.Basket/DigitalTrade.Basket.csproj"
COPY . .
WORKDIR "/src/DigitalTrade.Basket"
RUN dotnet build "DigitalTrade.Basket.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DigitalTrade.Basket.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalTrade.Basket.dll"]
