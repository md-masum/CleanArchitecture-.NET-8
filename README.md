# Bid2Drive

This is a vehicle rental platform where users can bid and rent vehicles.

## Project Overview

Bid2Drive is a full-stack application built with:
- **Backend:** ASP.NET Core (C#) following Clean Architecture principles, utilizing CQRS with MediatR, Entity Framework Core for data access, and ASP.NET Core Identity for authentication (including JWT, Google, and Facebook external logins). Serilog is used for structured logging to files.
- **Frontend:** (To be implemented, likely Next.js)

## Running with Docker

This project can be easily set up and run using Docker Compose.

**Prerequisites:**
- Docker Desktop (or Docker Engine) installed and running on your system.

**Steps:**

1.  **Navigate to the project root:**
    ```bash
    cd D:\Work\VehicleRentalPlatform\Bid2Drive
    ```

2.  **Build and run the Docker containers:**
    ```bash
    docker-compose up --build
    ```
    This command will:
    -   Build the Docker images for the SQL Server and the Bid2Drive API.
    -   Start the SQL Server container.
    -   Start the Bid2Drive API container, which will automatically apply database migrations and seed initial user data (SuperAdmin, Admin, Customer, Driver roles and users).

3.  **Access the API:**
    Once the containers are up and running, the Bid2Drive API will be accessible at `http://localhost:8080` (HTTP) and `https://localhost:8081` (HTTPS).
    You can access the Swagger UI for API documentation and testing at `http://localhost:8080/swagger` or `https://localhost:8081/swagger`.

4.  **Stop the containers:**
    To stop and remove the containers, networks, and volumes created by `docker-compose up`, run:
    ```bash
    docker-compose down
    ```

**Note on External Authentication:**
For Google and Facebook login to work, you need to replace the placeholder `YOUR_GOOGLE_CLIENT_ID`, `YOUR_GOOGLE_CLIENT_SECRET`, `YOUR_FACEBOOK_APP_ID`, and `YOUR_FACEBOOK_APP_SECRET` in `Backend/Bid2Drive.Api/appsettings.json` with your actual credentials obtained from Google and Facebook developer consoles.
