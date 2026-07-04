###########################################################
# MonoMaint.Host Dockerfile
###########################################################

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-azurelinux3.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0-azurelinux3.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ./src/MonoMaint.Host/MonoMaint.Host.csproj
RUN dotnet publish ./src/MonoMaint.Host/MonoMaint.Host.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Final image
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MonoMaint.Host.dll"]