# CleanArchitecture

A starter template for building robust ASP.NET Core applications following Clean Architecture principles, with complete authentication and authorization implemented out of the box.

## Overview

This template provides a scalable project structure built using:
- **Backend:** ASP.NET Core 8 (C#)
  - Clean Architecture pattern
  - CQRS using MediatR
  - Entity Framework Core
  - ASP.NET Core Identity with:
    - JWT Authentication
    - Google and Facebook external login support
  - Serilog for structured file logging

- **Frontend:** *(Not included; recommended to use with Next.js or any modern SPA framework)*

---

## Getting Started with Docker

This project is Docker-ready for quick local development and testing.

### Prerequisites
- Docker Desktop (or Docker Engine) installed and running

### Steps

1. **Navigate to the project root:**
   ```bash
   cd D:\Work\CleanArchitecture
   ```

2. **Build and run the Docker containers:**
   ```bash
   docker-compose up --build
   ```
   This will:
   - Build Docker images for SQL Server and the API
   - Start the SQL Server container
   - Start the API container and:
     - Apply EF Core migrations
     - Seed default roles and users (SuperAdmin, Admin, Customer, Driver)

3. **Access the API:**
   - HTTP: `http://localhost:8080`
   - HTTPS: `https://localhost:8081`
   - Swagger UI:  
     - `http://localhost:8080/swagger`  
     - `https://localhost:8081/swagger`

4. **Stop the containers:**
   ```bash
   docker-compose down
   ```

---

## External Authentication Setup

To enable Google and Facebook login, update the following placeholders in `Backend/CleanArchitecture.Api/appsettings.json`:

```json
"Google": {
  "ClientId": "YOUR_GOOGLE_CLIENT_ID",
  "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
},
"Facebook": {
  "AppId": "YOUR_FACEBOOK_APP_ID",
  "AppSecret": "YOUR_FACEBOOK_APP_SECRET"
}
```

Obtain credentials from the respective developer consoles:

- [Google Developer Console](https://console.developers.google.com)
- [Facebook Developer Console](https://developers.facebook.com)

---

## Features Included

- ✅ Modular Clean Architecture structure
- ✅ Built-in role-based authentication and authorization
- ✅ JWT, Google, and Facebook authentication
- ✅ Docker support with pre-configured SQL Server
- ✅ EF Core migrations and seeding
- ✅ Swagger UI for API testing
- ✅ Serilog file logging
