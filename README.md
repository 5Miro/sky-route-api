# SkyRoute API

ASP.NET Core backend for the SkyRoute demo. This repo provides the API for the flight search and booking slice using mocked provider integrations and in-memory booking confirmation.

## Features

- Airport and cabin-class lookups
- Flight search across mocked providers
- Booking confirmation with generated booking reference
- Route-based passenger document validation

## Prerequisites

- .NET 8 SDK

## Setup And Run

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project src/SkyRoute.Api
```

Run tests:

```bash
dotnet test
```

## Local Runtime Details

- Default HTTP development profile runs on `http://localhost:5012`
- Swagger UI is available in development at `http://localhost:5012/swagger`

## API Endpoints

- `GET /api/lookups/airports`
- `GET /api/lookups/cabin-classes`
- `POST /api/flights/search`
- `POST /api/bookings`

## Architecture

- Controllers expose HTTP endpoints and map requests/responses
- Application services coordinate search and booking behavior
- Domain types hold shared business concepts and rules
- Infrastructure contains provider mocks, lookups, in-memory repositories, and configuration
- Provider integrations are isolated behind `IFlightProviderClient`
- Document validation is isolated in `DocumentRuleService`
- Offer storage and booking persistence use in-memory implementations for demo scope

## Decisions

- Mocked providers were chosen to keep the challenge focused on architecture and flow, not real integrations
- Provider-specific behavior is separated from shared search logic for extensibility
- Validation and error handling are centralized through application services and middleware
- Swagger is enabled in development to make local API exploration easier

## Trade-Offs And Known Limitations

- Data is stored in memory only
- No database, authentication, payment, or external provider integrations are included
- CORS is tuned for local development scenarios
- Provider responses are mocked demo behavior, not real airline inventory or pricing
