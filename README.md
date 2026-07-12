# Base

<p align="center">
  <img src="./document.png" alt="Base Architecture" width="100%">
</p>

<p align="center">

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13-239120?logo=csharp)
![License](https://img.shields.io/github/license/MassoudKargar/Base)
![Last Commit](https://img.shields.io/github/last-commit/MassoudKargar/Base)
![Issues](https://img.shields.io/github/issues/MassoudKargar/Base)

</p>

<p align="center">
A production-ready foundation for building modern .NET applications using Clean Architecture, DDD, CQRS, and modular design.
</p>

---
# Overview

Base is a modern application framework for .NET that provides a robust starting point for building scalable, maintainable, and enterprise-grade applications.

It embraces **Clean Architecture**, **Domain-Driven Design (DDD)**, and **CQRS** to promote clear boundaries between business logic and infrastructure concerns. The framework includes reusable components, modular project organization, and production-ready conventions that help teams build software faster while maintaining high code quality.

Base is suitable for REST APIs, Modular Monoliths, and Microservice-based systems, making it an ideal foundation for both greenfield projects and enterprise solutions.
---

# Goals

- Clean Architecture by default
- Domain-Driven Design (DDD)
- CQRS support
- Dependency Injection
- Modular project structure
- Production-ready conventions
- High maintainability
- Testability
- Reusable infrastructure
- Extensible architecture

---

# Architecture

```
                   Client
                      │
                      ▼
              ASP.NET Core API
                      │
                      ▼
              Application Layer
                      │
         ┌────────────┴────────────┐
         ▼                         ▼
    Domain Layer          Infrastructure
         │                         │
         └────────────┬────────────┘
                      ▼
               External Services

        SQL Server
        Redis
        RabbitMQ
        Kafka
        gRPC
        Background Jobs
```

---

# Project Structure

```
Base
│
├── src
│   ├── Base.Domain
│   ├── Base.Application
│   ├── Base.Infrastructure
│   ├── Base.API
│   ├── Base.Shared
│   └── ...
│
├── samples
│
├── Utilities
│
├── docs
│
└── tests
```

---

# Layers

## Domain

Contains:

- Entities
- Value Objects
- Aggregates
- Domain Events
- Business Rules
- Interfaces

The Domain layer has **zero dependency** on any external technology.

---

## Application

Responsible for:

- CQRS
- Commands
- Queries
- DTOs
- Validation
- Business Workflows
- Authorization
- Mediation

---

## Infrastructure

Implements:

- Entity Framework
- Redis
- RabbitMQ
- Kafka
- Email
- SMS
- File Storage
- Background Jobs
- External APIs

Infrastructure depends on Domain—not the other way around.

---

## API

Contains:

- REST APIs
- Authentication
- Authorization
- Swagger
- Middleware
- Exception Handling
- Health Checks

---

# Features
- ✅ About → Website → https://www.nuget.org/packages/Base.Solution.Templates/
- ✅ Clean Architecture
- ✅ DDD
- ✅ CQRS
- ✅ Dependency Injection
- ✅ Fluent Validation
- ✅ Authentication
- ✅ Authorization
- ✅ JWT
- ✅ Swagger
- ✅ Global Exception Handling
- ✅ Logging
- ✅ Background Jobs
- ✅ Redis
- ✅ RabbitMQ
- ✅ Kafka Ready
- ✅ gRPC Ready
- ✅ Docker Ready
- ✅ OpenTelemetry Ready

---

# Technology Stack

| Technology | Purpose |
|------------|----------|
| .NET 10 | Framework |
| ASP.NET Core | Web API |
| C# | Language |
| Entity Framework Core | ORM |
| SQL Server | Database |
| Redis | Cache |
| RabbitMQ | Messaging |
| Kafka | Event Streaming |
| gRPC | Internal Communication |
| Docker | Containerization |
| OpenTelemetry | Observability |

---

# Design Principles

Base follows several important architectural principles:

- Separation of Concerns
- Single Responsibility
- Dependency Inversion
- Domain First
- Infrastructure Isolation
- Explicit Dependencies
- High Cohesion
- Low Coupling

---

# Why Base?

Many templates only generate folders.

Base provides:

- Consistent project organization
- Proven architectural patterns
- Reusable infrastructure
- Extensible modules
- Enterprise-ready conventions
- Better maintainability
- Faster project startup
- Easier onboarding for teams

---

# Typical Request Flow

```
Client
   │
   ▼
Controller / Endpoint
   │
   ▼
Application Layer
   │
   ▼
Domain
   │
   ▼
Infrastructure
   │
   ▼
Database / Cache / Queue
```

---

# Extensibility

The framework is designed to allow easy integration of:

- New Modules
- New Databases
- New Message Brokers
- New Cache Providers
- New Authentication Providers
- New External Services

without affecting the Domain layer.

---

# Getting Started

Clone the repository

```bash
git clone https://github.com/MassoudKargar/Base.git
```
Restore packages

```bash
dotnet restore
```

Build

```bash
dotnet build
```

Run

```bash
dotnet run
```

---

# Best Practices

When building on Base:

- Keep business rules inside Domain.
- Keep Controllers thin.
- Use CQRS for application workflows.
- Hide infrastructure behind abstractions.
- Avoid leaking Entity Framework into Application.
- Depend on interfaces, not implementations.

---

# Future Roadmap

- [ ] Native AOT support
- [ ] Multi-tenancy
- [ ] Event Sourcing
- [ ] Outbox Pattern
- [ ] Saga Support
- [ ] Modular Monolith template
- [ ] Microservices template
- [ ] Distributed Caching improvements
- [ ] Observability Dashboard
- [ ] CLI Project Generator

---

# Contributing

Contributions are welcome.

Feel free to submit issues, feature requests, or pull requests.

---

# License

This project is licensed under the **MIT License**.

---

# Author

**Masoud Kargar**

Senior .NET Backend Developer

GitHub:
https://github.com/MassoudKargar

LinkedIn:
https://www.linkedin.com/in/masoudkargar

---

<p align="center">
Made with ❤️ using .NET 10
</p>
