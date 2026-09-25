# KMC Event Platform

An event management and ticketing platform built with **ASP.NET Core**, made up of a REST API backend and a Razor Pages web front-end.

Repo: https://github.com/Soft-ware-tech/KMC-Event-Platform

## Repository structure

```
KMC_SOC/
└── KMC/
    ├── KMC_API/    # ASP.NET Core Web API — backend, database access
    └── KMCWeb/     # ASP.NET Core Razor Pages — public site, organizer & manager UI
```

## Projects

### KMC_API
The backend REST API. It handles authentication and data for:
- **Organizers** – registration/login, profile
- **Managers** – login, platform-wide oversight
- **Events** – create/read/update, categories, search
- **Ticket classes** – pricing and capacity per event
- **Registrations** – bookings, mock card payments
- **Activity log** – manager action history

KMCWeb's `Models/` folder mirrors this API's DTOs, so the two projects should be kept in sync when the API's contracts change.

### KMCWeb
The front-end. It calls KMC_API over HTTP (named `HttpClient` "KmcApi") and provides:
- **Public site** – browse events, view details, register/book tickets
- **Organizer back office** (`/BO`) – manage own events, ticket classes, and view attendees
- **Manager dashboard** (`/Manager`) – platform-wide stats, revenue by category, all bookings, category management

## Tech stack

- ASP.NET Core, .NET 10
- Razor Pages (KMCWeb) + Web API (KMC_API)
- Server-side session for organizer/manager login state
- SQL database (see `SQLQuery1.sql`)

## Getting started

1. Open `KMC.sln` (or the individual `.slnx` files) in Visual Studio.
2. Run **KMC_API** first — note the HTTPS port it starts on (default expected: `7101`).
3. If the port differs, update `ApiSettings:BaseUrl` in `KMCWeb/KMCWeb/appsettings.json`.
4. Run **KMCWeb**.
5. Browse events as a guest, or log in as an organizer (`/Login`) or manager (`/ManagerLogin`).

## License

Add a license here if this project is going to be public.
