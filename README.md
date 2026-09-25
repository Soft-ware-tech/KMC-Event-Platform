# KMCWeb

KMCWeb is an **ASP.NET Core Razor Pages** front-end for an event management and ticketing platform. It provides a public-facing site for browsing and booking events, an **Organizer** back office for managing events and tickets, and a **Manager** dashboard for platform-wide oversight and analytics.

> This project is the web front-end only. It communicates with a separate **KMC_API** backend over HTTP — the API must be running for KMCWeb to function.

## Features

### Public site
- Browse and search events by category (`Events`, `EventDetails`)
- Register / book tickets for an event, including seat-limited and ticketed (paid) events
- Mock card payment flow for ticketed events
- Booking confirmation page (`Confirmation`)

### Organizer back office (`/BO`)
- Organizer login/logout (session-based)
- Dashboard with an overview of the organizer's events
- Create, edit, and manage events (`ManageEvent`)
- Manage ticket classes and pricing (`ManageTickets`)
- View attendees/registrations per event (`Attendees`)

### Manager dashboard (`/Manager`)
- Manager login/logout (session-based)
- Platform-wide stats: total organizers, events, registrations, seats booked, and revenue
- Revenue and event counts broken down by category
- View all bookings across every organizer/event
- Manage event categories

## Tech stack

- **Framework:** ASP.NET Core Razor Pages, .NET 10
- **Backend integration:** `HttpClient` (named client `KmcApi`) calling the KMC_API REST backend
- **State:** Server-side session (`IDistributedCache` / `AddSession`) to track the logged-in organizer or manager
- **Frontend:** Razor views (`.cshtml`) with a shared layout

## Getting started

1. Ensure the **KMC_API** project is running (defaults to `https://localhost:7101/`).
2. If the API runs on a different port, update `ApiSettings:BaseUrl` in `appsettings.json`.
3. Run the project:
   ```bash
   dotnet run --project KMCWeb
   ```
4. Open the app in your browser and log in as an organizer (`/Login`) or manager (`/ManagerLogin`), or browse events as a guest from the home page.

## Project structure

```
KMCWeb/
├── Models/          # DTOs mirroring KMC_API's request/response models
├── Pages/
│   ├── BO/          # Organizer back-office pages
│   ├── Manager/     # Manager dashboard pages
│   ├── Shared/      # Shared layout
│   └── *.cshtml     # Public pages (Index, Events, Register, Login, etc.)
├── Program.cs       # App startup, session + HttpClient configuration
└── appsettings.json # API base URL configuration
```
