# Web API + Blazor UI Scaffold

Complete scaffold for adding deployable Web API + Blazor UI to the BonusAccumulator solution.

---

## 1. Assumptions

- **`IWordService.Anagram/Build/Pattern/Distance`** are synchronous, stateless-per-call methods that return `Answer` (confirmed from source).
- **`ITrieSearcher`, `TrieNode`, `IAnagramTrieBuilder`, `ILazyLoadingTrie`, `LazyLoadingTrie`** are all immutable after initialization and thread-safe for concurrent reads. They are registered as singletons.
- **`ISessionState` / `SessionState`** is mutable per-user state (tracks `SessionWords`, `AddedWords`, `LastResult`). In the CLI it was singleton (one user). For web it **must be scoped** (one per HTTP request / SignalR circuit).
- **`IWordService` / `WordService`** holds a reference to `ISessionState` and `ITrieSearcher`. Because `ISessionState` must be scoped, `IWordService` must also be scoped (a scoped service cannot be injected into a singleton).
- **`RunQuiz` and `RunChainQuiz`** use `Action<string>` / `Func<string?>` callbacks (console I/O). These cannot be directly exposed as request/response endpoints. The quiz endpoints are **stubbed** with TODO markers indicating the adapter pattern needed.
- **`ISettingsProvider.GetSetting`** returns `string?` and reads from `IConfiguration`. The existing `ConfigurationSettingsProvider` in `BonusAccumulator` is host-specific; each new host gets its own identical implementation (`WebConfigurationSettingsProvider`).
- **`CardboxDataLayer.DependencyInjection.AddCardboxDataLayer`** already registers `CardboxDbContext` (scoped), `IQuestionRepository` (scoped), all analytics query interfaces (scoped), and `IAnalyticsService` (scoped). This is called as-is from the new hosts.
- **`IAnalyticsService`** implements `IGetDeckStatsByCardbox` which has `Task<IEnumerable<CardboxStats>> ExecuteAsync()`. The `/api/cardbox/stats` endpoint calls this.
- **Dictionary file** lives at a local path (`C:\Lexicon\list.txt`). Deployment will need this file accessible to the host process.
- **SQLite database** is at a local file path. Same deployment consideration applies.

---

## 2. Recommended Architecture

### Option A — Single Host (WordServices.Host)

One ASP.NET Core project serving both minimal API endpoints and Blazor Server interactive UI. The Blazor pages call services directly via DI (no HTTP round-trip for same-host). API endpoints are still available for external consumers, testing, and future decoupling.

**Pros**: Simplest deployment, single process, no CORS, one config file, easiest debugging.
**Cons**: UI and API scale together; harder to independently deploy API for other clients later.

### Option B — Split Host (WordServices.Api + WordServices.Web)

Separate API-only project and Blazor Web App project. The Blazor app calls the API via `HttpClient`. CORS is configured on the API. Each project deploys independently.

**Pros**: Clean separation, independent scaling, API reusable by mobile/other clients.
**Cons**: Two processes to run locally, CORS config, slightly more complex deployment.

**Recommendation**: Start with Option A for fastest path. Migrate to Option B when you need independent scaling or multiple API consumers.

---

## 3. Solution Tree

```
BonusAccumulator/
├── BonusAccumulator.sln
├── README.md
├── SCAFFOLD.md
├── AI_GUIDANCE.md                          (existing)
├── .github/
│   └── workflows/
│       └── build-and-test.yml              ★ NEW
│
├── BonusAccumulator/                       (existing CLI host — unchanged)
│   ├── BonusAccumulator.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── ConfigurationSettingsProvider.cs
│   └── WordServicesDependencyInjection.cs
│
├── WordServices/                           (existing core — unchanged)
│   ├── WordServices.csproj
│   ├── IWordService.cs
│   ├── WordService.cs
│   ├── Answer.cs
│   ├── ISettingsProvider.cs
│   ├── ISessionState.cs
│   ├── SessionState.cs
│   ├── QuizOptions.cs
│   ├── TrieNode.cs
│   ├── Output/
│   ├── TrieLoading/
│   ├── TrieSearching/
│   ├── Analytics/
│   └── Extensions/
│
├── CardboxDataLayer/                       (existing data — unchanged)
│   ├── CardboxDataLayer.csproj
│   ├── CardboxDbContext.cs
│   ├── DependencyInjection.cs
│   ├── Entities/
│   └── Repositories/
│
├── WordServices.Host/                      ★ NEW — Option A (single host)
│   ├── WordServices.Host.csproj
│   ├── Program.cs
│   ├── requests.http
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Configuration/
│   │   ├── WebConfigurationSettingsProvider.cs
│   │   └── WordServicesDependencyInjection.cs
│   ├── Dtos/
│   │   ├── AnagramSearchRequest.cs
│   │   ├── AnagramSearchResponse.cs
│   │   ├── SearchMode.cs
│   │   ├── QuizStartRequest.cs
│   │   ├── QuizStartResponse.cs
│   │   ├── QuizAnswerRequest.cs
│   │   ├── QuizAnswerResponse.cs
│   │   ├── QuizModeDto.cs
│   │   ├── CardboxStatsResponse.cs
│   │   └── CardboxBucketDto.cs
│   ├── Endpoints/
│   │   ├── HealthEndpoints.cs
│   │   ├── AnagramEndpoints.cs
│   │   ├── QuizEndpoints.cs
│   │   └── CardboxEndpoints.cs
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Components/
│   │   ├── App.razor
│   │   ├── Routes.razor
│   │   ├── _Imports.razor
│   │   ├── Layout/
│   │   │   └── MainLayout.razor
│   │   └── Pages/
│   │       ├── Home.razor
│   │       └── Stats.razor
│   └── wwwroot/
│       └── css/
│           └── app.css
│
├── WordServices.Api/                       ★ NEW — Option B (API only)
│   ├── WordServices.Api.csproj
│   ├── Program.cs
│   ├── requests.http
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Configuration/
│   │   ├── WebConfigurationSettingsProvider.cs
│   │   └── WordServicesDependencyInjection.cs
│   ├── Dtos/
│   │   ├── (same as Host)
│   ├── Endpoints/
│   │   ├── (same as Host)
│   └── Middleware/
│       └── GlobalExceptionMiddleware.cs
│
├── WordServices.Web/                       ★ NEW — Option B (Blazor frontend)
│   ├── WordServices.Web.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Models/
│   │   ├── AnagramSearchRequest.cs
│   │   ├── AnagramSearchResponse.cs
│   │   ├── SearchMode.cs
│   │   ├── CardboxStatsResponse.cs
│   │   └── CardboxBucketDto.cs
│   ├── Components/
│   │   ├── App.razor
│   │   ├── Routes.razor
│   │   ├── _Imports.razor
│   │   ├── Layout/
│   │   │   └── MainLayout.razor
│   │   └── Pages/
│   │       ├── Home.razor
│   │       └── Stats.razor
│   └── wwwroot/
│       └── css/
│           └── app.css
│
├── WordServicesTests/                      (existing — unchanged)
└── CardboxDataLayerTests/                  (existing — unchanged)
```

---

## 4. dotnet CLI Commands

Run from the solution root (`BonusAccumulator/`):

```powershell
# Create Option A — single host
dotnet new web -n WordServices.Host -o WordServices.Host --framework net10.0
# (then replace generated files with the scaffold files above)

# Create Option B — API
dotnet new webapi -n WordServices.Api -o WordServices.Api --framework net10.0
# (then replace generated files with the scaffold files above)

# Create Option B — Blazor Web
dotnet new blazor -n WordServices.Web -o WordServices.Web --framework net10.0 --interactivity Server
# (then replace generated files with the scaffold files above)

# Add project references
dotnet add WordServices.Host/WordServices.Host.csproj reference WordServices/WordServices.csproj
dotnet add WordServices.Host/WordServices.Host.csproj reference CardboxDataLayer/CardboxDataLayer.csproj

dotnet add WordServices.Api/WordServices.Api.csproj reference WordServices/WordServices.csproj
dotnet add WordServices.Api/WordServices.Api.csproj reference CardboxDataLayer/CardboxDataLayer.csproj

# Add all new projects to solution
dotnet sln BonusAccumulator.sln add WordServices.Host/WordServices.Host.csproj
dotnet sln BonusAccumulator.sln add WordServices.Api/WordServices.Api.csproj
dotnet sln BonusAccumulator.sln add WordServices.Web/WordServices.Web.csproj
```

Since the scaffold files are already generated in place, you only need the `dotnet sln add` and `dotnet add reference` commands. The `dotnet new` steps are optional (the .csproj files are already created).

**Minimal commands (since files exist):**

```powershell
dotnet sln BonusAccumulator.sln add WordServices.Host/WordServices.Host.csproj
dotnet sln BonusAccumulator.sln add WordServices.Api/WordServices.Api.csproj
dotnet sln BonusAccumulator.sln add WordServices.Web/WordServices.Web.csproj
```

---

## 5. Generated Code Files

All code files have been generated directly into the solution tree. See the Solution Tree (section 3) for the complete list. Key files:

### Option A (WordServices.Host)

| File | Purpose |
|---|---|
| `Program.cs` | ASP.NET Core host wiring API + Blazor |
| `Configuration/WordServicesDependencyInjection.cs` | Registers WordServices with scoped session state |
| `Configuration/WebConfigurationSettingsProvider.cs` | ISettingsProvider backed by IConfiguration |
| `Endpoints/AnagramEndpoints.cs` | `POST /api/anagrams/search` |
| `Endpoints/HealthEndpoints.cs` | `GET /api/health` + detail |
| `Endpoints/QuizEndpoints.cs` | Stubbed quiz start/answer |
| `Endpoints/CardboxEndpoints.cs` | `GET /api/cardbox/stats` |
| `Middleware/GlobalExceptionMiddleware.cs` | ProblemDetails exception handler |
| `Dtos/*.cs` | Request/response DTOs with validation |
| `Components/Pages/Home.razor` | Anagram search UI (calls IWordService directly) |
| `Components/Pages/Stats.razor` | Cardbox statistics UI |

### Option B (WordServices.Api + WordServices.Web)

| File | Purpose |
|---|---|
| `Api/Program.cs` | API-only host with CORS |
| `Api/Endpoints/*.cs` | Same endpoints as Host |
| `Web/Program.cs` | Blazor host with HttpClient to API |
| `Web/Components/Pages/Home.razor` | Calls API via HttpClient |
| `Web/Components/Pages/Stats.razor` | Calls API via HttpClient |
| `Web/Models/*.cs` | Client-side DTOs |

### Existing Files — No Changes Required

The existing `BonusAccumulator`, `WordServices`, and `CardboxDataLayer` projects are **not modified**. The new hosts wire into them via:
- `CardboxDataLayer.DependencyInjection.AddCardboxDataLayer(configuration)` — called as-is
- A new `AddWordServicesForWeb` extension method that mirrors the CLI's `AddWordServices` but with **scoped** `ISessionState` and `IWordService`

---

## 6. How It Wires Into Existing Projects

### DI Registrations and Lifetimes

```
SINGLETON (immutable, thread-safe, loaded once):
  ISettingsProvider          → WebConfigurationSettingsProvider
  IWordOutputService         → DefaultWordOutputService
  TrieNode                   → TrieNode
  IAnagramTrieBuilder        → AnagramTrieBuilder
  ILazyLoadingTrie           → LazyLoadingTrie
  ITrieSearcher              → TrieSearcher

SCOPED (per-request / per-circuit):
  ISessionState              → SessionState         ← CHANGED from singleton
  IWordService               → WordService           ← CHANGED from singleton
  CardboxDbContext            → (EF Core scoped)     ← already scoped
  IQuestionRepository        → QuestionRepository    ← already scoped
  IAnalyticsService          → AnalyticsService      ← already scoped
  All IGet* analytics        → (already scoped)
```

### Why ISessionState/IWordService Changed to Scoped

In the CLI, `SessionState` was singleton because there was one user. In a web app:
- **Multiple concurrent users** would share and corrupt a singleton `SessionState`
- `SessionState.SessionWords` / `AddedWords` / `LastResult` are mutable `HashSet`/`List` — not thread-safe
- `WordService` calls `sessionState.Update()` on every search — must be per-request

For Option A (Blazor Server), scoped means **per SignalR circuit** — each browser tab gets its own session state, which is the correct behavior.

### Integration Points

1. **`AddCardboxDataLayer(configuration)`** — Called from `Program.cs` exactly as in the CLI. No changes needed to `CardboxDataLayer.DependencyInjection`.

2. **`AddWordServicesForWeb(configuration)`** — New extension method in each host project. Mirrors the CLI's `AddWordServices` but:
   - Uses `WebConfigurationSettingsProvider` instead of `ConfigurationSettingsProvider`
   - Registers `ISessionState` and `IWordService` as **scoped** instead of singleton

3. **`ISettingsProvider`** — Each host has its own `WebConfigurationSettingsProvider` (identical to the CLI's `ConfigurationSettingsProvider` but in the host's namespace). This avoids coupling the new hosts to the `BonusAccumulator` namespace.

### Where Assumptions May Need Adapting

- If `TrieSearcher` or `LazyLoadingTrie` has any mutable state beyond the initial load, they may need to be scoped too. Review their source to confirm thread safety.
- If `WordService` has additional state beyond what's shown (e.g., `_unasked` HashSet seen in source), this state will be per-request in the scoped model. If you need cross-request state, consider a separate `IUserSessionStore` with `ConcurrentDictionary`.

---

## 7. Local Run Instructions

### Option A

```powershell
# From solution root
dotnet sln BonusAccumulator.sln add WordServices.Host/WordServices.Host.csproj
dotnet build BonusAccumulator.sln
dotnet run --project WordServices.Host/WordServices.Host.csproj
```

| URL | What |
|---|---|
| https://localhost:7200 | Blazor UI |
| https://localhost:7200/swagger | Swagger UI |
| https://localhost:7200/api/health | Health check |
| https://localhost:7200/stats | Cardbox stats page |

Test with the `.http` file in `WordServices.Host/requests.http` (open in VS / VS Code REST Client / Rider).

### Option B

Terminal 1:
```powershell
dotnet run --project WordServices.Api/WordServices.Api.csproj
```

Terminal 2:
```powershell
dotnet run --project WordServices.Web/WordServices.Web.csproj
```

| URL | What |
|---|---|
| https://localhost:7250/swagger | API Swagger |
| https://localhost:7300 | Blazor UI |

### curl Examples

```bash
# Health
curl https://localhost:7200/api/health/detail

# Anagram search
curl -X POST https://localhost:7200/api/anagrams/search \
  -H "Content-Type: application/json" \
  -d "{\"rack\":\"SATIRE\",\"mode\":\"Anagram\"}"

# Build search
curl -X POST https://localhost:7200/api/anagrams/search \
  -H "Content-Type: application/json" \
  -d "{\"rack\":\"SATIRE\",\"mode\":\"Build\"}"

# Cardbox stats
curl https://localhost:7200/api/cardbox/stats
```

---

## 8. Deployment Starter Plan

### Simple First Deployment (Single Host — Option A)

**Target**: Single Windows server or Azure App Service.

1. Publish:
   ```powershell
   dotnet publish WordServices.Host/WordServices.Host.csproj -c Release -o ./publish/host
   ```

2. Copy the `publish/host` folder to the target machine.

3. Ensure the dictionary file (`DictionaryPath`) and SQLite database (`ConnectionStrings:CardboxDatabase`) are accessible from the target machine. Update paths in `appsettings.json` or set environment variables:
   ```
   DictionaryPath=/data/lexicon/list.txt
   ConnectionStrings__CardboxDatabase=Data Source=/data/cardbox/Anagrams.db
   ```

4. Run:
   ```powershell
   dotnet WordServices.Host.dll
   ```

5. For IIS: Use the ASP.NET Core Module. For Azure App Service: deploy the publish folder via ZIP deploy or GitHub Actions artifact.

6. HTTPS: Use Kestrel behind a reverse proxy (IIS, nginx) with a real certificate, or let Azure App Service handle TLS.

### Scalable Split-Host (Option B)

1. Publish API and Web separately:
   ```powershell
   dotnet publish WordServices.Api/WordServices.Api.csproj -c Release -o ./publish/api
   dotnet publish WordServices.Web/WordServices.Web.csproj -c Release -o ./publish/web
   ```

2. Deploy API to one host/container, Web to another.

3. Configure `ApiBaseUrl` in the Web project's `appsettings.json` to point to the API's public URL.

4. Configure `Cors:AllowedOrigins` in the API's `appsettings.json` to include the Web project's public URL.

5. Scale API independently (multiple instances behind a load balancer). The trie is loaded per-instance (memory-heavy but immutable). Consider a shared dictionary file mount.

6. For containers: Each project gets its own Dockerfile. The dictionary file and SQLite DB should be volume-mounted.

### GitHub Actions

The `.github/workflows/build-and-test.yml` workflow handles build, test, and publish artifact creation for all three new projects on push to `main`.

---

## 9. Common Pitfalls / Review Checklist

### CLI-to-Web Migration Pitfalls

- **Singleton mutable state**: The biggest risk. `SessionState` was singleton in CLI. Must be scoped for web. Already handled in the scaffold, but verify no other singletons hold mutable per-user state.

- **Trie memory**: The trie is loaded into memory once (singleton). For a 200K+ word dictionary, this can be 50-200MB. Ensure the deployment target has enough RAM. The trie loads on first `ILazyLoadingTrie` access — first request may be slow (cold start).

- **Console I/O in service code**: `RunQuiz`, `RunChainQuiz`, `ConvertToAlphagrams`, `AddWords` all take `Action<string>` / `Func<string?>` parameters for console I/O. These are **not directly usable** from a web endpoint. The scaffold stubs the quiz endpoints. To fully implement:
  1. Create an `IQuizSessionService` that breaks the quiz loop into individual question/answer steps
  2. Store quiz state in a scoped service or distributed cache (keyed by session ID)
  3. Expose `GET /api/quiz/{sessionId}/next` and `POST /api/quiz/{sessionId}/answer`

- **File paths**: `DictionaryPath` and `SessionOutputPath` are absolute Windows paths. These will break on Linux containers. Use environment variables to override per environment.

- **SQLite concurrency**: SQLite has limited write concurrency. Fine for single-user or read-heavy scenarios. If you get `database is locked` errors under load, consider switching to SQL Server or PostgreSQL for production.

- **`SessionState.SaveAdded()` writes to disk**: The `StoreAndClearAdded` command writes to a local file. This needs adapting for web (e.g., return the content as a download, or store in the database).

### Review Checklist

- [ ] Verify `TrieSearcher` and `LazyLoadingTrie` are thread-safe for concurrent reads
- [ ] Verify `TrieNode` is immutable after build
- [ ] Confirm `DictionaryPath` is accessible from the deployment target
- [ ] Confirm SQLite DB path is accessible and writable (if any write operations exist)
- [ ] Test with a large dictionary to measure cold-start time and memory usage
- [ ] Verify `IGetDeckStatsByCardbox.ExecuteAsync()` is the correct method signature on `IAnalyticsService` for the cardbox stats endpoint (the interface inherits from many sub-interfaces)
- [ ] Decide on quiz endpoint implementation strategy (request/response vs WebSocket vs SignalR)
- [ ] Review `WordService._unasked` field — it's a mutable HashSet that persists across calls in the same scope. In scoped lifetime it resets per-request, which may or may not be desired
- [ ] Add authentication/authorization if deploying publicly
- [ ] Add rate limiting for the search endpoint if deploying publicly
- [ ] Consider adding response caching for anagram results (same rack always produces same result)
- [ ] Test that the Blazor Server circuit correctly scopes `ISessionState` per tab/user
