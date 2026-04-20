# Gym Portal

## Description

This project is a gym portal designed for both prospective and existing members, as well as administrators. Users can create accounts, sign in using either local credentials or third-party authentication, and access features relevant to their role. Access to certain pages and functionality is restricted to signed-in members only.

Members can update their profile information, sign up for memberships, book training sessions, cancel existing bookings, and remove their account if desired. Administrators have additional privileges, including access to an admin dashboard where they can manage the system, create new training sessions, and delete existing ones.

The system includes support for authentication and authorization using ASP.NET Identity, enabling role-based access control between members and administrators. This ensures a clear separation of responsibilities and access within the application.

The main purpose of this project is educational. It was built to explore and practice software architecture concepts such as Clean Architecture, Domain-Driven Design (DDD), and SOLID principles. The focus is on structuring the application into clear layers, managing dependencies, and designing a maintainable and scalable system.

The project also includes unit and integration testing using xUnit to validate core application logic and ensure reliability across key components.

<div align="center">
    <img width="640" alt="gymportal-preview" src="https://github.com/user-attachments/assets/c65d8295-0455-4095-b093-56ffdb462bf4" />
</div>

## Getting Started

Live demo: https://gymportal-crl96.azurewebsites.net/

### Run and build locally

#### Prerequisites

- .NET SDK

#### Installation

1. Clone the repository

2. Navigate to the project directory and startup project:

```terminal
cd GymPortal/Presentation.WebApp
```

3. Add database connection strings to appsettings or secrets

```terminal
"ConnectionStrings": {
    "SqlConnection": "YourProdDBString",
    "DevDbConnection": "YourDevDBString"
}
```

4. Add the following secrets

```terminal
"Authentication": {
    "GitHub": {
      "ClientId": "YourClientId",
      "ClientSecret": "YourClientSecret"
    }
},
"DefaultAdminPassword": "SuperSecretPassword"
```

5. Build or run the project:

```terminal
dotnet build
dotnet run
```

Or launch it in your IDE of choice.

## Technologies Used

Language: C#

Framework: ASP.NET MVC

Authentication: ASP.NET Identity (with support for external providers)

Testing: xUnit
