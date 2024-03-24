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

- **Database:** 
  - SQL Server 2019 or any
  - Run Database Script from Database Folder

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
  - DefaultConnection: SQL Server Database Connection[Data Source=[Server Name];Initial Catalog=[DB Name]; Integrated Security=[True];TrustServerCertificate=[True]; UID=[user id];Password=[password]
- **AppSettings:**
  - ApiUrl: url of the api[https://localhost:7001/], used in unit testing.
- **CommandTimeout:** Database Command Timeout (300)
- **TotalParkingSpots:** Number of Parking Spots (15)
- **HourlyFee:** Hourly rate per car (15)

## DateTime
- **Check In, Check Out, and Other Time:** Stored in UTC time in the Database, and in the frontend, showing client time.

## Security
- No Authorization and Authentication implemented (Out of scope).

# Application Layers

## 1. ParkingManagementApp 

### a. Purpose: 
This layer serves as the entry point of the application, responsible for managing HTTP requests, controllers, views, and routes. It directly interacts with the Application.

### b. Responsibilities: 
- Define API routes and corresponding actions. 
- Handle incoming HTTP requests and delegate processing to controllers. 
- Manage the overall flow of the application. 
- Manage Middleware.
- Manage Views, JavaScript, and CSS.

### c. Benefits: 
- **Separation of Concerns**: Focuses solely on handling HTTP requests, promoting a clean and modular architecture. 
- **Maintainability**: Changes in controller/view behavior can be made without affecting the underlying business logic. 

### d. Subgroups:
- Views: Handles all views, including shared views.
- Controllers: Manages all controllers.
- Middleware: Manages all middleware components.
- ApplicationExtensions: Contains application-related extensions.

## 2. ParkingManagementApp.Common 

### a. Purpose: 
This layer contains shared components, utilities, DTOs, extensions, and constants, promoting code reuse and encapsulating common functionality. 

### b. Responsibilities: 
- Define common data structures, constants, and Enums. 
- Implement utility functions for reuse throughout the application. 

### c. Benefits: 
- **Reusability**: Centralizes common functionality, reducing redundancy. 
- **Consistency**: Ensures consistency in data structures and utility functions. 

## 3. ParkingManagementApp.Core 

### a. Purpose: 
The Core layer contains the core business logic, representing the essential functionality of the application. 

### b. Responsibilities: 
- Define business entities. 
- Implement business rules and logic. 
- Interact with data repositories for data retrieval or persistence. 

### c. Benefits: 
- **Separation of Concerns**: Isolates business logic from API and data access layers. 
- **Testability**: Enables independent unit testing of business logic.

## 4. ParkingManagementApp.Model 

### a. Purpose: 
Responsible for the application's data, business logic, and rules, encapsulating the structure and behavior of data entities.

### b. Responsibilities: 
- Define data entities/domain objects. 
- Implement business logic/rules for data manipulation, validation, and processing. 
- Provide methods to interact with data entities.

### c. Benefits: 
- **Separation of Concerns**: Separates data and domain logic from the presentation layer. 
- **Flexibility**: Allows changes to the data structure without affecting other parts of the application.

## 5. ParkingManagementApp.Data 

### a. Purpose: 
Handles data access and persistence, interacting with the database or other data storage mechanisms.

### b. Responsibilities: 
- Implement data access logic, including CRUD operations. 
- Interact with the database using ADO.NET. 

### c. Benefits: 
- **Separation of Concerns**: Isolates data access logic, allowing changes to the database schema independently. 
- **Scalability**: Optimizable for performance independent of other layers.

## 6. ParkingManagementApp.Infrastructure 

### a. Purpose: 
Supports the overall infrastructure of the application, including services, external integrations, and cross-cutting concerns.

### b. Responsibilities: 
- Implement services for infrastructure-level functionality. 
- Manage external dependencies and integrations. 
- Handle cross-cutting concerns like authentication, caching, or logging.

### c. Benefits: 
- **Modularity**: Isolates infrastructure concerns for easy updates or replacements. 
- **Maintainability**: Changes to infrastructure components don't affect business logic or other layers.

## 7. ParkingManagementApp.Tests 

### a. Purpose: 
Dedicated to testing, including unit tests, integration tests, and other testing-related components.

### b. Responsibilities:
- Implement unit tests for individual components. 
- Write integration tests to validate interaction between different layers. 
- Ensure overall quality and reliability of the application.

### c. Benefits: 
- **Quality Assurance**: Ensures each component and layer functions as expected. 
- **Code Stability**: Provides a safety net when making changes or adding new features.

# Summary
This architecture emphasizes separation of concerns, maintainability, testability, and scalability. Each layer has a specific purpose and defined responsibilities, contributing to a robust and extensible ASP.NET MVC application.