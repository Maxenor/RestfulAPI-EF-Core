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
- **Pagination and Filtering**: Efficiently navigate large data sets with optimized queries
- **Comprehensive Error Handling**: Structured error responses with appropriate HTTP status codes
- **Input Validation**: Robust validation of all input data

## Technologies and Technical Choices

- **Framework**: ASP.NET Core 8
  - *Why*: Chosen for its performance, cross-platform capabilities, and robust ecosystem for building modern web APIs
  
- **ORM**: Entity Framework Core 8
  - *Why*: Provides a powerful and flexible ORM with clean LINQ integration, allowing for type-safe queries and migrations

- **Database**: MariaDB (deployed via Docker)
  - *Why*: Selected for its compatibility with MySQL, performance, and open-source nature

- **Documentation**: Swagger/OpenAPI
  - *Why*: Offers automatic, interactive API documentation that simplifies testing and client integration

- **Architecture**: Clean Architecture
  - *Why*: Promotes separation of concerns, testability, and independence from frameworks and UI

- **Other tools**: 
  - **AutoMapper**: Eliminates repetitive object-to-object mapping code
  - **Docker**: Ensures consistent development and deployment environments
  - **xUnit**: Provides a flexible testing framework for unit tests

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
Contains enterprise business rules and entities that are independent of external frameworks.

### Application Layer
Contains business rules specific to the application, DTOs, interfaces, and service implementations.

### Infrastructure Layer
Contains implementations of the interfaces defined in the application layer, such as database access.

### API Layer
Contains controllers, middleware, and API models for the presentation layer.

## Data Model

The system is built around the following key entities and their relationships:

```
Event <──┬─1:N─── Session <───┬─N:M─── Speaker
         │                    │
         │                    1:N
         │                    │
         │                    ▼
         │                   Rating
         │                    ▲
         │                    │
         ├─1:N─── Room        1:N
         │        ▲           │
         │        │           │
         │        N:1         │
         │        │           │
         ├─N:1─── Location    │
         │                    │
         ├─N:1─── Category    │
         │                    │
         └─N:M─── Participant ┘
```

- **Events** have multiple sessions, belong to a category and location
- **Sessions** are linked to an event and room, can have multiple speakers
- **Participants** can register for multiple events and provide ratings for sessions
- **Speakers** can present at multiple sessions
- **Locations** contain multiple rooms where sessions take place

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

## API Endpoints and Examples

The API provides the following main endpoints:

### Events
- `GET /api/v1/events` - List all events with optional filtering
- `GET /api/v1/events/{id}` - Get a specific event
- `POST /api/v1/events` - Create a new event
- `PUT /api/v1/events/{id}` - Update an event
- `DELETE /api/v1/events/{id}` - Delete an event

#### Example: Creating an Event

**Request:**
```http
POST /api/v1/events
Content-Type: application/json

{
  "title": "Tech Conference 2025",
  "description": "Annual gathering of tech professionals",
  "startDate": "2025-10-15T09:00:00Z",
  "endDate": "2025-10-17T17:00:00Z",
  "categoryId": 1,
  "locationId": 1,
  "status": "Published"
}
```

**Response:**
```http
Status: 201 Created
Location: /api/v1/events/10

{
  "id": 10,
  "title": "Tech Conference 2025",
  "description": "Annual gathering of tech professionals",
  "startDate": "2025-10-15T09:00:00Z",
  "endDate": "2025-10-17T17:00:00Z",
  "status": "Published",
  "category": {
    "id": 1,
    "name": "Technology"
  },
  "location": {
    "id": 1,
    "name": "Conference Center A",
    "address": "123 Main St",
    "city": "New York",
    "country": "USA"
  }
}
```

### Participants
- `GET /api/v1/participants` - List all participants
- `GET /api/v1/participants/{id}` - Get a specific participant
- `POST /api/v1/participants` - Register a new participant
- `PUT /api/v1/participants/{id}` - Update a participant
- `DELETE /api/v1/participants/{id}` - Delete a participant
- `POST /api/v1/events/{eventId}/participants/{participantId}` - Register a participant for an event
- `DELETE /api/v1/events/{eventId}/participants/{participantId}` - Unregister a participant from an event

#### Example: Registering a Participant for an Event

**Request:**
```http
POST /api/v1/events/1/participants/2
Content-Type: application/json
```

**Response:**
```http
Status: 204 No Content
```

For a complete list of endpoints, refer to the Swagger documentation.

## Testing

The project includes unit tests to verify the correctness of the application logic.

### Running the Tests

```bash
# Navigate to the tests directory
cd tests/EventManagement.UnitTests

# Run the tests
dotnet test
```

### Test Coverage

Tests focus on the application service layer, verifying that business rules are correctly applied and that the services interact properly with their dependencies.

## Challenges Encountered and Solutions

### Challenge 1: Complex Data Relationships
Implementing the many-to-many relationships (e.g., Event-Participant, Session-Speaker) required careful consideration of the entity configurations and navigation properties.

**Solution**: Used Fluent API for explicit entity configurations and created join entities with additional properties to represent these relationships.

### Challenge 2: Query Performance Optimization
As the database grew, query performance became a concern, especially for complex queries involving multiple related entities.

**Solution**: Implemented pagination, added strategic indexes, and used projection queries to select only the required data.

### Challenge 3: Consistent Error Handling
Maintaining consistent error responses across different controllers proved challenging.

**Solution**: Created a global exception handling middleware and standardized error response formats using Problem Details.

### Challenge 4: Transaction Management
Ensuring data consistency for operations that modify multiple entities required careful transaction management.

**Solution**: Implemented a Unit of Work pattern to coordinate transactions across multiple repositories.

## Future Improvements

- Authentication and authorization using JWT
- Caching frequently accessed data
- Implementation of real-time notifications
- Advanced reporting and analytics features
- GraphQL API alongside REST for more flexible queries