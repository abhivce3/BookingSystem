# BookingSystem
Booking System is a gRPC-based microservices application built with .NET 8, ASP.NET Core, and MediatR, designed to manage bookings, members, and inventory efficiently. It follows clean architecture, leveraging gRPC for high-performance communication, Entity Framework Core for database access, and MediatR for CQRS.

🌟 Features
✅ gRPC-based Microservices – High-performance, scalable service communication
✅ Booking Management – Create, cancel, and retrieve available bookings(Not Cancelled) 
✅ Inventory Management – Import inventory data and retrieve inventory details
✅ Member Management – Import members and fetch member details
✅ CQRS with MediatR – Decouples business logic using commands and queries
✅ Entity Framework Core – Uses SQL Server for database operations
✅ Docker Support – Easily deployable with Docker & Kubernetes

📂 Project Structure

📦 BookingSystem
 ┣ 📂 BookingSystem.Application       # Business Logic Layer
 ┃ ┣ 📂 Config                        # Application configurations
 ┃ ┣ 📂 DTOs                          # Data Transfer Objects
 ┃ ┣ 📂 Features                      # CQRS (Commands & Queries)
 ┃ ┣ 📂 Interfaces                    # Repository & service interfaces
 ┃ ┣ 📂 Mapping                       # AutoMapper profiles
 ┃ ┣ 📂 Parsers                       # CSV parsing utilities
 ┃ ┣ 📜 DependencyInjection.cs        # Dependency injection for Application layer
 ┣ 📂 BookingSystem.Domain            # Core Business Entities (Domain Layer)
 ┃ ┣ 📂 Entities                      # Core domain models
 ┣ 📂 BookingSystem.Infrastructure    # Data Persistence Layer
 ┃ ┣ 📂 Migrations                    # EF Core Migrations
 ┃ ┣ 📂 Persistence                   # Database Context & Configurations
 ┃ ┣ 📂 Repositories                  # Repository Implementations
 ┃ ┣ 📜 DependencyInjection.cs        # Dependency injection for Infrastructure
 ┣ 📂 BookingSystem.Service           # gRPC Service Layer
 ┃ ┣ 📂 Behaviors                     # gRPC Middleware Behaviors
 ┃ ┣ 📂 Middleware                    # Global middleware handlers
 ┃ ┣ 📂 Protos                        # gRPC Protocol Buffers (Proto Files)
 ┃ ┣ 📂 Services                      # gRPC Service Implementations
 ┃ ┣ 📜 appsettings.json              # Configuration settings
 ┃ ┣ 📜 Program.cs                    # Main entry point
 ┣ 📂 BookingSystem.Tests             # Unit & Integration Testing
 ┃ ┣ 📂 Service                       # gRPC Service Tests
 ┃ ┣ 📜 GlobalUsings.cs               # Common using statements for tests
 ┃ ┣ 📜 TestServerCallContext.cs      # Mocked gRPC ServerCallContext for tests

 🛠️ Setup & Installation
📌 Prerequisites
Ensure you have the following installed before running the application:

1️⃣ Install Go (Required for gRPC UI)
🔹 Download and install Go from:
🔗 https://go.dev/dl/

2️⃣ Install gRPC UI (Optional - For Testing)
To install the gRPC UI client, run:
go install github.com/fullstorydev/grpcui/cmd/grpcui@latest

3️⃣ Install Global Entity Framework (EF) Tool
Run the following command to install EF Core CLI globally:
dotnet tool install --global dotnet-ef
If already installed, update it using:
dotnet tool update --global dotnet-ef

🛠️ Database Setup
Run the following commands to create and update the database using EF Core Migrations:
dotnet ef database update

📌 Running the Application
Open Terminal
path/to_your/BookingSystem.Service> dotnet run

🚀 Running the Application Locally
1️⃣ Open Command Prompt (CMD) in the Solution Directory
Navigate to the root directory of the solution where .sln file is located.

2️⃣ Start the gRPC UI for API Testing
grpcui -plaintext localhost:5131







 



