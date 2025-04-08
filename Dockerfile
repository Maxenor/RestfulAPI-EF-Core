# Use the official ASP.NET Core runtime image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["RestfulAPI.csproj", "."]
RUN dotnet restore "./RestfulAPI.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR "/src/."

# Build the application
RUN dotnet build "./RestfulAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestfulAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entry point for the container
ENTRYPOINT ["dotnet", "RestfulAPI.dll"]