# NotesApi Backend

A simple ASP.NET Core Web API for managing notes with user authentication. This project is part of a full-stack notes application (see the `front-end` directory for the Vue 3 client).

## Features
- User registration and login (with password hashing)
- CRUD operations for notes
- SQL Server database (Entity Framework Core + Dapper)
- OpenAPI/Swagger documentation
- CORS enabled for frontend integration

## Project Structure
```
back-end/
  Controllers/      # API controllers (Auth, Notes)
  Data/             # Entity Framework DbContext
  Models/           # Data models (Note, User)
  Repositories/     # Data access (Dapper)
  appsettings.json  # Configuration
  Program.cs        # App entry point
  ...
```

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or remote)

## Setup & Run
1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd back-end
   ```
2. **Configure the database**
   - Edit `appsettings.json` and set your SQL Server connection string under `ConnectionStrings:DefaultConnection`.
   - Example:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NotesDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
     ```
3. **Apply database migrations**
   > _Note: This project uses Entity Framework Core for migrations and Dapper for data access._
   ```bash
   dotnet ef database update
   ```
4. **Run the API**
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:5001` (or as configured).

5. **API Documentation**
   - Swagger UI is available at `/swagger` when running in Development mode.

## API Endpoints
### Auth
- `POST /api/auth/register` — Register a new user
- `POST /api/auth/login` — Login with username and password

### Notes
- `GET /api/notes` — List all notes
- `GET /api/notes/{id}` — Get a note by ID
- `POST /api/notes` — Create a new note
- `PUT /api/notes/{id}` — Update a note
- `DELETE /api/notes/{id}` — Delete a note

## Models
### User
- `Id` (int)
- `Username` (string)
- `PasswordHash` (string)
- `CreatedAt` (DateTime)

### Note
- `Id` (int)
- `Title` (string)
- `Content` (string?)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime?)
- `UserId` (int)

## Configuration
- `appsettings.json` contains logging and connection string settings.
- Use `appsettings.Development.json` for local overrides.

## Database Schema (Manual Setup)

If you prefer to create the database and tables manually in SQL Server (instead of using EF Core migrations), use the following SQL scripts.

**Database name:** `NotesDb`

```sql
CREATE DATABASE NotesDb;
GO

USE NotesDb;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Notes (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255) NOT NULL,
    Content TEXT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    UserId INT NOT NULL 
);
```

## Updating & Extending
- **To add new endpoints:**
  - Create a new controller in `Controllers/`.
  - Register new services in `Program.cs` if needed.
- **To update the database:**
  - Modify models in `Models/` and run EF Core migrations.
- **To update dependencies:**
  - Run `dotnet add package <PackageName>` and update `NotesApi.csproj`.
- **To update the frontend:**
  - See `front-end/README.md` for Vue client setup.

## Useful Commands
- Run the API: `dotnet run`
- Add migration: `dotnet ef migrations add <MigrationName>`
- Update DB: `dotnet ef database update`
- List packages: `dotnet list package`

For the Vue 3 frontend, see the `front-end` directory and its README for setup instructions. 