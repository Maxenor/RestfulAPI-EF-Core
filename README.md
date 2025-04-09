# Event Management System API

## Overview

The Event Management System API is a RESTful web service designed for managing professional events such as conferences, trade shows, and workshops. This platform provides a comprehensive solution for handling all aspects of event organization, including event creation, participant registration, session scheduling, speaker management, venue coordination, and more.

## Features

### Core Functionality
- **Event Management**: Create, update, delete, and retrieve events with detailed filtering options
- **Participant Management**: Register/unregister participants, track attendance, view participation history
- **Session Management**: Organize sessions within events, assign speakers and rooms
- **Speaker Management**: Track event speakers, their sessions, and roles
- **Venue Management**: Manage event locations and room assignments
- **Category Management**: Organize events by categories

### Advanced Features
- **Rating System**: Allow participants to rate and comment on sessions
- **Statistical Reports**: Generate insights on event attendance and participation
- **Transaction Management**: Handle financial transactions related to event registration

## Technologies

- **Framework**: ASP.NET Core 8
- **ORM**: Entity Framework Core 8
- **Database**: MariaDB (deployed via Docker)
- **Documentation**: Swagger/OpenAPI
- **Architecture**: Clean Architecture / Multi-layer architecture
- **Other tools**: 
  - AutoMapper for entity-DTO mapping
  - Docker for containerization
  - FluentValidation for input validation
  - xUnit for unit testing

## Architecture

The application follows a clean architecture approach with clear separation of concerns:

```
src/
├── Domain/            # Core business entities and logic
├── Application/       # Use cases, DTOs, and service interfaces
├── Infrastructure/    # Data access and external services implementation
└── API/               # API controllers, middleware, and configuration
```

### Domain Layer
Contains enterprise business rules and entities.

### Application Layer
Contains business rules specific to the application, DTOs, interfaces, and service implementations.

### Infrastructure Layer
Contains implementations of the interfaces defined in the application layer, such as database access.

### API Layer
Contains controllers, middleware, and API models for the presentation layer.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker and Docker Compose
- Git

### Installation

1. Clone the repository
```bash
git clone https://github.com/Maxenor/event-management-api.git
cd event-management-api
```

2. Start the application with Docker Compose
```bash
docker compose up -d
```

This will start both the API and the MariaDB database in containers.

3. Access the API
   - API: https://localhost:5002
   - Swagger Documentation: https://localhost:5003/swagger

### Manual Development Setup

1. Update the database connection string in `appsettings.json` if not using Docker
2. Run the database migrations
```bash
dotnet ef database update
```
3. Run the application
```bash
dotnet run
```

## API Endpoints

The API provides the following main endpoints:

### Events
- `GET /api/events` - List all events with optional filtering
- `GET /api/events/{id}` - Get a specific event
- `POST /api/events` - Create a new event
- `PUT /api/events/{id}` - Update an event
- `DELETE /api/events/{id}` - Delete an event

### Participants
- `GET /api/participants` - List all participants
- `GET /api/participants/{id}` - Get a specific participant
- `POST /api/participants` - Register a new participant
- `PUT /api/participants/{id}` - Update a participant
- `DELETE /api/participants/{id}` - Delete a participant
- `POST /api/events/{eventId}/participants/{participantId}` - Register a participant for an event
- `DELETE /api/events/{eventId}/participants/{participantId}` - Unregister a participant from an event

For a complete list of endpoints, refer to the Swagger documentation.