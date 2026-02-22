# BonusAccumulator — WordServices Solution

Scrabble/anagram tool with CLI, Web API, and Blazor UI.

## Projects

| Project | Description |
|---|---|
| **BonusAccumulator** | Original CLI host |
| **WordServices** | Core logic: anagram/trie search, quiz, session, analytics |
| **CardboxDataLayer** | EF Core SQLite: DbContext, entities, repositories |
| **WordServices.Host** | **Option A** — Single ASP.NET Core host (API + Blazor UI) |
| **WordServices.Api** | **Option B** — Standalone Web API |
| **WordServices.Web** | **Option B** — Standalone Blazor Web App (calls API via HttpClient) |
| **WordServicesTests** | Unit tests for WordServices |
| **CardboxDataLayerTests** | Unit tests for CardboxDataLayer |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Dictionary file at the path configured in `appsettings.json` (`DictionaryPath`)
- SQLite database at the path configured in `ConnectionStrings:CardboxDatabase`

## Quick Start

### Option A — Single Host (recommended for local dev)

```powershell
cd WordServices.Host
dotnet run
```

- **UI**: https://localhost:7200
- **Swagger**: https://localhost:7200/swagger
- **Health**: https://localhost:7200/api/health

### Option B — Split Host

Terminal 1 (API):
```powershell
cd WordServices.Api
dotnet run
```

Terminal 2 (Web):
```powershell
cd WordServices.Web
dotnet run
```

- **API Swagger**: https://localhost:7250/swagger
- **Web UI**: https://localhost:7300

### CLI (unchanged)

```powershell
cd BonusAccumulator
dotnet run
```

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/health` | Health check |
| GET | `/api/health/detail` | Health detail with timestamp |
| POST | `/api/anagrams/search` | Anagram/build/pattern search |
| POST | `/api/quiz/start` | Start quiz session (stubbed) |
| POST | `/api/quiz/answer` | Submit quiz answer (stubbed) |
| GET | `/api/cardbox/stats` | Cardbox statistics |

## Sample curl

```bash
curl -X POST https://localhost:7200/api/anagrams/search \
  -H "Content-Type: application/json" \
  -d '{"rack":"SATIRE","mode":"Anagram"}'
```

## Configuration

Settings are read from `appsettings.json`. Override with environment variables:

```
DictionaryPath=C:\Lexicon\list.txt
ConnectionStrings__CardboxDatabase=Data Source=path\to\Anagrams.db
```

## Build and Test

```powershell
dotnet build BonusAccumulator.sln
dotnet test BonusAccumulator.sln
```

## Publish

```powershell
dotnet publish WordServices.Host/WordServices.Host.csproj -c Release -o ./publish/host
```
