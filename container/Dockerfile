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

RUN dotnet restore ./src/Hosts/MonoMaint.Host.Sample/MonoMaint.Host.Sample.csproj
RUN dotnet publish ./src/Hosts/MonoMaint.Host.Sample/MonoMaint.Host.Sample.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Final image
FROM base AS final
WORKDIR /app

# Docker / Render向け設定を使用する
ENV DOTNET_ENVIRONMENT=Container

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MonoMaint.Host.Sample.dll"]