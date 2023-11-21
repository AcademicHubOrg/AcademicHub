#!/bin/bash

# Start Docker Compose for PostgreSQL
echo "Starting PostgreSQL for CourseTemplate Service"
cd ..
cd CourseTemplate
docker-compose up -d

# Wait for PostgreSQL to be ready

sleep 3
echo "PostgreSQL is ready!"

# Apply EF Core migrations 

cd CourseTemplate.Data
dotnet ef database update


# Run the .NET Solution 
cd ..
cd CourseTemplate
export ASPNETCORE_URLS=https://localhost:7155
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore CourseTemplate.sln
dotnet run CourseTemplate.sln &