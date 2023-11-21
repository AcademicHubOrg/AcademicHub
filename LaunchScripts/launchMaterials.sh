#!/bin/bash

# Start Docker Compose for PostgreSQL
echo "Starting PostgreSQL for Materials Service"
cd ..
cd Materials
docker-compose up -d

# Wait for PostgreSQL to be ready
sleep 3
echo "PostgreSQL is ready!"

# Apply EF Core migrations

cd Materials.Data
dotnet ef database update


# Run the .NET Solution
cd ..
cd Materials
export ASPNETCORE_URLS=https://localhost:7263
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore Materials.sln
dotnet run Materials.sln &