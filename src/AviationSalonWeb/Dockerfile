FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AviationSalonWeb/AviationSalon.WebUI.csproj", "AviationSalonWeb/"]
RUN dotnet restore "./AviationSalonWeb/AviationSalon.WebUI.csproj"
COPY . .
WORKDIR "/src/AviationSalonWeb"
RUN dotnet build "./AviationSalon.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AviationSalon.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install --global dotnet-ef --version 8.0.0

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ["dotnet", "AviationSalon.WebUI.dll", "migrate"]
