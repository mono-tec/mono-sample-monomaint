# MonoMaint Sample

MonoMaint Sample is a reference implementation of the MonoMaint maintenance platform.

This repository provides a minimal application for developing and testing MonoMaint plugins.

## Features

- .NET 10 LTS
- Blazor Server
- Docker / Docker Compose
- GitHub Actions
- Plugin-based architecture

## Project Structure

```text
.
├── .github/
├── container/
├── installer/
│   ├── ubuntu/
│   └── windows/
├── src/
├── tests/
└── MonoMaint.sln
```

## Getting Started

### Run locally

```bash
dotnet run --project src/MonoMaint.Host
```

### Run with Docker

```bash
cd container
docker compose up --build
```

## License

MIT License