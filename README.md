# Downgrooves.Core5

The backend for [downgrooves.com](https://www.downgrooves.com): a .NET 9 Web API that serves the site's music catalog, plus a background worker that keeps the release catalog in sync with the iTunes Search API.

The site itself is an Angular app in a separate repo: [djericj/Downgrooves.Angular](https://github.com/djericj/Downgrooves.Angular).

## About Downgrooves

Downgrooves.com is my personal music project website. I've been a DJ and music producer for over 25 years and have built up a large collection of original music, DJ sets and YouTube performance videos. The site brings all of it together in one place, covering the artist names I release under: **Downgrooves**, **Eric Rylos** and **Evotone**.

## Architecture at a glance

```
          iTunes Search / Lookup API
                    │
                    ▼
     ┌───────────────────────────────┐
     │  Downgrooves.WorkerService    │  polls iTunes, saves raw JSON,
     │  (systemd background service) │  downloads cover artwork
     └───────────────┬───────────────┘
                     │ writes
                     ▼
   Json/  (flat JSON data store)        CDN folder (artwork, mp3s)
                     │ reads
                     ▼
     ┌───────────────────────────────┐
     │  Downgrooves.WebApi           │  read-only REST API
     │  → Service → Data → Domain    │
     └───────────────┬───────────────┘
                     ▼
         Downgrooves.Angular (www.downgrooves.com)
```

There is no database. Everything the API serves comes from JSON files on disk (see [Why no database?](#why-no-database)).

## Solution layout

| Project | Description |
|---|---|
| **Downgrooves.Domain** | Plain model classes shared by every project: `Artist`, `Release`, `ReleaseTrack`, `Mix`, `MixTrack`, `Genre`, `Video`, `Thumbnail`, `User`, `AppConfig`, plus DTOs for raw iTunes (`ITunes/`) and YouTube (`YouTube/`) API responses. |
| **Downgrooves.Data** | Data access objects (`ArtistDao`, `GenreDao`, `MixDao`, `ReleaseDao`, `UserDao`, `VideoDao`) that load and query the JSON files. `DaoFactory` creates them all. `ReleaseAdapter` turns raw iTunes collection and track JSON into `Release`/`ReleaseTrack` objects. File and folder names live in `Types/`. |
| **Downgrooves.Service** | Business layer between the controllers and the DAOs (`ArtistService`, `MixService`, `ReleaseService`, `VideoService`, `GenreService`, `UserService`) plus `TokenService` for building and validating JWTs. |
| **Downgrooves.WebApi** | ASP.NET Core host: controllers, CORS, JWT authentication, Swagger, ProblemDetails error handling and Serilog logging. |
| **Downgrooves.WorkerService** | Long-running `BackgroundService` that pulls data and artwork from iTunes. It runs under systemd on Linux (`UseSystemd()`). |
| **Downgrooves.Test** | MSTest unit tests for mixes, releases and videos (with Moq / NSubstitute). |
| **Json/** | The data store: hand-maintained catalog files plus the iTunes data the worker downloads. |
| **deploy/** | Deployment assets, such as the systemd unit file for the worker. |

## Web API

All controllers except `ConfigController` require authorization (`[Authorize]`, JWT bearer). In the **Development** environment authorization is turned off by `DisableAuthenticationPolicyEvaluator`, so you can call every endpoint locally without a token.

| Method | Route | Returns |
|---|---|---|
| GET | `/artists` | All artists |
| GET | `/genres` | All genres |
| GET | `/mixes` | All DJ mixes, newest first |
| GET | `/mixes/category?category={category}` | Mixes in a category |
| GET | `/mixes/genre?genre={genre}` | Mixes in a genre |
| GET | `/mix/{id}` | One mix with its track list |
| GET | `/releases/{artistName?}` | All releases, or one artist's releases, newest first |
| GET | `/release/{id}` | One release (iTunes collection ID) with its tracks |
| GET | `/videos` | All YouTube videos |
| GET | `/video/{id}` | One video with its thumbnails |
| GET | `/config/env` | The current hosting environment name (no auth) |
| GET | `/config/exception` | Logs and returns a test 500 error (no auth) |

The API rewrites media fields to absolute URLs on the configured CDN (`AppConfig:CdnUrl`) before returning them:

- Mix artwork → `{CdnUrl}/images/mixes/...`, mix audio → `{CdnUrl}/mp3/...`
- Release artwork → `{CdnUrl}/images/artwork/...`
- Video artwork → `{CdnUrl}/images/artwork/videos/{youtubeId}/high.jpg`, video link → `https://youtu.be/{youtubeId}`

When running in Development, Swagger UI is at `/swagger`.

### Release filtering

`ReleaseDao` builds the release catalog from every `Json/itunes/{collectionId}.json` file and leaves out:

- collection IDs listed in `AppConfig:Exclusions:CollectionIds`
- titles containing any string in `AppConfig:Exclusions:Keywords` (for example `"- Single"` or `"(DJ Mix)"`)

## Worker service

`ProcessWorker` runs a loop every `AppConfig:PollInterval` seconds (21600 by default, which is 6 hours). Each pass:

1. **CheckFolders**: creates `Json/itunes/collections/artists` and `Json/itunes/tracks/artists` if they are missing.
2. **GetDataFromITunesApi**: if the last check was more than 24 hours ago (tracked by a `last_checked_*.txt` file), or any search file is missing, searches iTunes for each artist (Downgrooves, Evotone, Eric Rylos). It saves both collection and track search results under those folders.
3. **GetData**: pulls every `collectionId` out of the search results. For each ID that doesn't have a `Json/itunes/{id}.json` file yet, it calls the iTunes Lookup API and saves the album with its tracks, waiting `ITunes:LookupInterval` seconds between calls to stay within rate limits.
4. **GetArtwork**: for each release without a matching `{id}.jpg` in `ArtworkBasePath`, downloads the 500×500 cover image.
5. **WriteLastCheckedFile**: records the time of this check.

If a pass throws an exception, the worker sets a non-zero exit code and stops. The systemd unit then restarts it.

## Data store (`Json/`)

| File / folder | Contents |
|---|---|
| `artist.json` | Artist profiles |
| `genre.json` | Genre list |
| `mixes.json`, `mixTracks.json` | DJ mixes and their track lists |
| `video.json`, `thumbnail.json` | YouTube videos and their thumbnails |
| `siteVersion.json` | Site version information |
| `itunes/{collectionId}.json` | Raw iTunes lookup results, one file per release (written by the worker) |
| `itunes/collections/artists/`, `itunes/tracks/artists/` | Raw iTunes search results per artist (written by the worker) |
| `users/user.json` | API users (git-ignored) |
| `last_checked_*.txt` | When the worker last searched iTunes |

`Json/itunes/` and `Json/users/` are git-ignored. They're generated or private, so a fresh clone won't include them.

## Why no database?

There used to be a SQLite database behind the API. However, most of this site is static data that rarely changes. Only the Releases and Remixes sections change regularly, and both get their data from the iTunes API, which already returns JSON. The worker stores that JSON as-is, and the API reads it directly.

Parsing the iTunes JSON and then inserting, updating and deleting the matching database rows turned out to be far more error-prone than just keeping the JSON files. For a dataset this small and simple, the database was an unnecessary layer.

## Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A local copy of the `Json/` data folder, and optionally a local CDN folder for artwork and audio

### Configuration

Both apps read `appsettings.json`, then user secrets (Web API only), then environment variables. Update the machine-specific paths before you run them.

**Downgrooves.WebApi/appsettings.json**

| Setting | Purpose |
|---|---|
| `AppConfig:WebAppUrls` | Origins allowed by CORS (the Angular dev server and so on) |
| `AppConfig:CdnUrl` | Base URL used when building media URLs |
| `AppConfig:MediaBasePath` | Local path to the CDN media folder |
| `AppConfig:JsonDataBasePath` | Path to the `Json/` folder |
| `AppConfig:Exclusions` | Release collection IDs and title keywords to hide |
| `Jwt:Key`, `Jwt:Issuer` | JWT signing key and issuer. Override these with user secrets or environment variables outside local development. |

**Downgrooves.WorkerService/appsettings.json**

| Setting | Purpose |
|---|---|
| `AppConfig:JsonDataBasePath` | Path to the `Json/` folder the worker writes to |
| `AppConfig:ArtworkBasePath` | Where downloaded cover art is saved |
| `AppConfig:PollInterval` | Seconds between worker passes |
| `AppConfig:ITunes:*` | iTunes search and lookup URL templates, and the delay between lookups |

Environment variables use `__` for nesting, for example `AppConfig__JsonDataBasePath=/var/www/json`.

### Build, run and test

```bash
dotnet build Downgrooves.Core5.sln

# Web API (http://localhost:5000, Swagger at /swagger in Development)
dotnet run --project Downgrooves.WebApi

# Worker service
dotnet run --project Downgrooves.WorkerService

# Tests
dotnet test
```

## CI/CD and deployment

The code is built and released through Azure DevOps, with a local Development server and an AWS-hosted Production environment. Both run on Linux to keep hosting fast and cheap.

| Pipeline | Triggered by changes to | What it does |
|---|---|---|
| `azure-pipelines.yml` | WebApi, Service, Data, Domain | Builds, runs the tests, publishes the Web API artifact |
| `azure-pipelines-worker-service.yaml` | WorkerService, Domain | Publishes the worker artifact |
| `azure-pipelines-json-data.yml` | any push to `main` | Packages the `Json/` folder as an artifact |
| `azure-pipelines-nuget.yml` | Domain | Builds the Domain library |
| `azure-pipelines-admin.yaml` | Downgrooves.Admin | Publishes the Admin app (that project is not in this repo) |

On the server, the worker runs as a systemd service. To install it, copy [deploy/downgrooves-worker.service](deploy/downgrooves-worker.service) to `/etc/systemd/system/`, adjust `WorkingDirectory` and `ExecStart` to match your deployment, then run:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now downgrooves-worker
```
