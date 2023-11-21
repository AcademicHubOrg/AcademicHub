#!/bin/bash

# Start Docker Compose for PostgreSQL
echo "Starting PostgreSQL for CourseStream Service"
cd ..
cd CourseStream
docker-compose up -d

sleep 3
echo "PostgreSQL is ready!"


# Apply EF Core migrations (Ensure you're in the correct directory to do this)

cd CourseStream.Data
dotnet ef database update


# Run the .NET Solution (Ensure this is the correct directory and method to start your application)
cd ..
cd CourseStream
export ASPNETCORE_URLS=https://localhost:7237
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore CourseStream.sln
dotnet run CourseStream.sln &