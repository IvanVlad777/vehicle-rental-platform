# Vehicle Rental Platform

## How to Run the Solution

The application is composed of two services:

- **ASP.NET Core Web API** (`vehicleapi`)
- **Microsoft SQL Server** (`sqlserver`)

First, you need to have **Docker** and **Docker Compose** installed.
Virtualization must be enabled in BIOS.
WSL 2 must be properly installed and configured (on Windows systems).

### Steps to Run

1. Open a terminal and navigate to the project root folder where the `docker-compose.yml` file is located.
2. Run the following command to build and start all services:
   ```bash
   docker-compose up --build
   ```
3. After the containers are started, the API will be available at:
   ```
   http://localhost:8080/swagger
   ```
4. To stop the containers, run:
   ```bash
   docker-compose down
   ```

### Ports

- SQL Server is exposed on port `1433`.
- Web API is exposed on port `8080`.

---

## Architecture and Design Decisions

### Architecture

The solution follows a clean architecture structure, split into several projects:

- **Domain**: Core business models and logic.
- **Application**: Application services and interfaces.
- **Infrastructure**: Database context, migrations, and external service implementations.
- **API**: ASP.NET Core Web API, configuration. This one is entry point.

### Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core 9.0
- SQL Server 2022
- Docker, Docker Compose
- AutoMapper for object mapping
- JWT for authentication

### Database

- The SQL Server container runs with an initial password configured in the `docker-compose.yml` file.
- Entity Framework Core applies database migrations automatically at application startup.
- Initial data seeding (vehicles, telemetry, users) is performed during startup.

### Security

- JWT authentication is used to protect API endpoints.
- HTTPS redirection is enabled internally (note: Docker by default exposes HTTP unless configured otherwise).

---

## Additional Useful Commands

### Running Migrations

If you need to add new migrations manually, run:

```bash
dotnet ef migrations add MigrationName --project ./VehicleRentalPlatform.Infrastructure --startup-project ./VehicleRentalPlatform.API
```

Apply migrations manually if needed:

```bash
dotnet ef database update --project ./VehicleRentalPlatform.Infrastructure --startup-project ./VehicleRentalPlatform.API
```

### Seeding Data

At application startup, initial data is automatically seeded from:

- `/Files/vehicles.csv`
- `/Files/telemetry.csv`
- Default users are seeded into the database.

## Authentication and Using Swagger

### Obtaining a JWT Token

To access protected API endpoints, you first need to obtain a JWT token:

1. Go to the `/api/Auth/login` endpoint in Swagger. (User for demonstration: admin@vrp.com, admin123.)
2. Provide valid login credentials (e.g., email and password of a seeded user).
3. Execute the request.
4. Copy the returned token from the response.

### Using the Token in Swagger

1. Click on the "Authorize" button in Swagger (top right of web app).
2. Enter the token in the input field.
3. After authorization, you will be able to call secured API endpoints.

---
