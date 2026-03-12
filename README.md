# IMovie

A REST API for managing movies and ratings, built with ASP.NET Core and PostgreSQL.

## Architecture

```
Movies.Api              → Controllers, auth, Swagger, middleware
Movies.Application      → Services, repositories, validators, database
Movies.Contracts        → Request/response DTOs (shared between API and SDK)
Movies.Api.Sdk          → Refit-based client SDK
Movies.Api.Sdk.Consumer → SDK usage example
```

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- PostgreSQL + Dapper
- FluentValidation
- JWT + API Key authentication
- Output caching
- API versioning (media type)
- Refit (SDK)

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker

### Run the Database

```bash
cd Movies.Application
docker-compose up -d
```

### Run the API

```bash
cd Movies.Api
dotnet run
```

The API runs at `https://localhost:5001`. Swagger UI is available at `https://localhost:5001/swagger`.

## API Endpoints

### Movies

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/movies` | TrustedMember | Create a movie |
| GET | `/api/movies/{idOrSlug}` | Anonymous | Get a movie by ID or slug |
| GET | `/api/movies` | Anonymous | Get all movies (filterable, sortable, paginated) |
| PUT | `/api/movies/{id}` | TrustedMember | Update a movie |
| DELETE | `/api/movies/{id}` | Admin | Delete a movie |

### Ratings

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| PUT | `/api/movies/{id}/ratings` | Authenticated | Rate a movie (1-5) |
| DELETE | `/api/movies/{id}/ratings` | Authenticated | Delete your rating |
| GET | `/api/ratings/me` | Authenticated | Get your ratings |

### Auth

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/token` | Generate a JWT token |

## Authentication

**JWT Bearer** — pass token in `Authorization: Bearer <token>` header.

**API Key** — pass key in `x-api-key` header (for service-to-service calls).

### Generate a Token

```bash
POST https://localhost:5001/token
Content-Type: application/json

{
  "userId": "d8566de3-b1a6-4a9b-b842-8e3887a82e41",
  "email": "user@example.com",
  "customClaims": {
    "admin": true,
    "trusted_member": true
  }
}
```

## Query Parameters (Get All Movies)

| Parameter | Example | Description |
|-----------|---------|-------------|
| title | `?title=matrix` | Filter by title (partial match) |
| year | `?year=1999` | Filter by release year |
| sortBy | `?sortBy=-title` | Sort field (prefix `-` for descending) |
| page | `?page=1` | Page number |
| pageSize | `?pageSize=10` | Results per page (max 25) |

## SDK Usage

```csharp
services.AddMoviesApiSdk("https://localhost:5001");

var moviesApi = provider.GetRequiredService<IMoviesApi>();
var movies = await moviesApi.GetMoviesAsync(new GetAllMoviesRequest
{
    Page = 1,
    PageSize = 10
});
```

## Health Check

```
GET https://localhost:5001/_health
```

## License

MIT
