#!/bin/bash

# Start Docker Compose for PostgreSQL
echo "Starting PostgreSQL for Identity Service"
cd Identity
docker-compose up -d
cd ..

# Wait for PostgreSQL to be ready (Optional, you might need a more sophisticated wait mechanism)
sleep 3

# Apply EF Core migrations (Ensure you're in the correct directory to do this)
cd Identity
cd Identity.Data
dotnet ef database update
cd ..

# Run the .NET Solution (Ensure this is the correct directory and method to start your application)
cd Identity
export ASPNETCORE_URLS=https://localhost:7108
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore Identity.sln
dotnet run Identity.sln




echo "Starting CourseTemplate Service"
cd CourseTemplate
docker-compose up -d
cd ..

echo "Starting CourseStream Service"
cd CourseStream
docker-compose up -d
cd ..

echo "Starting Materials Service"
cd Materials
docker-compose up -d
cd ..

echo "Starting Frontend"
cd frontend
# Insert commands to start the React frontend, e.g., `npm start`
cd ..