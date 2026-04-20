# Proiect Marsh

Full-stack Device Management application with:
- Backend: ASP.NET Core 8 Web API
- Frontend: Angular 19
- Database: SQL Server / SQL Server LocalDB

This README is the single setup guide for both projects.

## Repository Structure

- backend/Marasescu_Lucian_Project_Task
- backend/Marasescu_Lucian_Project_Task.IntegrationTests
- frontend

## Prerequisites

Install these before starting:

- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server LocalDB (or SQL Server + SSMS / Azure Data Studio)

Optional tools:
- SQL Server Management Studio (SSMS)
- Azure Data Studio

## Quick Start

1. Create and seed the database (see Database Setup below).
2. Start backend API.
3. Start frontend app.
4. Open http://localhost:4200

Backend default URL:
- http://localhost:5199

Frontend default URL:
- http://localhost:4200

## Backend Setup

Project folder:
- backend/Marasescu_Lucian_Project_Task

### Restore and run

From the backend project folder, run:

    dotnet restore
    dotnet run --launch-profile http

Alternative (HTTPS profile):

    dotnet run --launch-profile https

### Backend configuration

Add the development connection string and the Gemini Token in:
- appsettings.Development.json

Default database name used by scripts and config:
- DeviceManagementDb

The frontend is expected to call backend on:
- http://localhost:5199

## Database Setup

Database scripts are in:
- backend/Marasescu_Lucian_Project_Task/scripts

Run scripts in this exact order:

1. 01_create_database.sql
2. 02_create_tables.sql
3. 03_seed_data.sql
4. 04_device_assignments_table.sql
5. 05_seed_device_assignments.sql
6. 06_add_auth_columns.sql

### Run with SSMS or Azure Data Studio

1. Connect to your SQL instance (LocalDB or SQL Server).
2. Open each script in order.
3. Execute each script and wait for success before running the next.

### Verify database setup

After scripts run:
- Database DeviceManagementDb exists
- Tables exist: Devices, Users, DeviceAssignments
- Seed data exists in Users and Devices
- Auth columns exist in Users: Email, PasswordHash

## Frontend Setup

Project folder:
- frontend

### Install and run

From frontend folder, run:

    npm install
    npm start

Open:
- http://localhost:4200

### Build

    npm run build

## Integration Tests

Integration tests are located in:
- backend/Marasescu_Lucian_Project_Task.IntegrationTests

### Run integration tests

From repository root:

    dotnet test backend/Marasescu_Lucian_Project_Task.IntegrationTests/Marasescu_Lucian_Project_Task.IntegrationTests.csproj

Or from the integration tests folder:

    dotnet test

## Integration and Local Development Notes

- Frontend services are configured to call backend at http://localhost:5199.
- If you run backend on a different port, update frontend service base URLs accordingly.
- Backend CORS is configured for Angular dev origins on port 4200.
