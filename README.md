# Project Guideline and Used Technology

## Architecture
- **Layer Architecture:** The project follows a layered architecture pattern for organizing code into distinct layers, promoting separation of concerns and modularity.

## Technology
- **Frontend:**
  - Razor view page
  - JavaScript
  - CSS
  - Bootstrap Library

- **Backend:**
  - ASP.NET Core MVC
  - ADO.NET

- **Database:** SQL Server 2019

- **Testing:**
  - xUnit
  - Selenium for frontend testing

## Design Patterns
- **MVC Pattern**
- **Unity of Work Pattern**
- **Dependency Injection**
- **Adapter Pattern**
- **Middleware Pattern**
- **Singleton Pattern**
- **Factory Method Pattern**
- **Repository Pattern**

## Testing
- **Unit Testing:** xUnit
- **Frontend Testing:** Selenium

## NuGet Packages
- System.Data.SqlClient
- ADO.Net.Client.Core
- Microsoft.Extensions.Configuration
- Newtonsoft.Json
- xunit
- xunit.runner.visualstudio
- Moq
- Microsoft.NET.Test.Sdk
- Selenium.WebDriver
- Selenium.WebDriver.ChromeDriver
- Bogus

## Appsettings (appsettings.json)
- **ConnectionStrings:**
  - DefaultConnection: SQL Server Database Connection
- **AppSettings:**
  - ApiUrl: URL of the API (used in unit testing)
- **CommandTimeout:** Database Command Timeout (300)
- **TotalParkingSpots:** Number of Parking Spots (15)
- **HourlyFee:** Hourly rate per car (15)

## DateTime
- **Check In, Check Out, and Other Time:** Stored in UTC time in the Database, and in the frontend, showing client time.

## Security
- No Authorization and Authentication implemented (Out of scope).

## Application Layers

1. **ParkingManagementApp**
   - **Purpose:** Entry point responsible for handling HTTP requests, managing controllers, views, defining routes, and managing middleware.
   - **Subgroups:**
     - Views
     - Controllers
     - Middleware
     - ApplicationExtensions

2. **ParkingManagementApp.Common**
   - **Purpose:** Contains shared components, utilities, DTOs, extensions, and constants promoting code reuse and encapsulation of common functionality.

3. **ParkingManagementApp.Core**
   - **Purpose:** Core business logic layer containing business entities, rules, logic, and interactions with data repositories.

4. **ParkingManagementApp.Model**
   - **Purpose:** Responsible for the application's data, business logic, and rules, encapsulating the structure and behavior of data entities.

5. **ParkingManagementApp.Data**
   - **Purpose:** Responsible for data access and persistence, implementing data access logic and interacting with the database using ADO.NET.

6. **ParkingManagementApp.Infrastructure**
   - **Purpose:** Contains components supporting the overall infrastructure of the application, including services, external integrations, and cross-cutting concerns.

7. **ParkingManagementApp.Tests**
   - **Purpose:** Dedicated to testing, including unit tests, integration tests, and other testing-related components.

## Summary
This architecture emphasizes modularity, maintainability, testability, and scalability by following a clear separation of concerns and organizing code into distinct layers. Each layer has defined responsibilities, contributing to a robust and extensible ASP.NET MVC application.
