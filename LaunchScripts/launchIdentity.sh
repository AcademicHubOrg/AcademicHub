#!/bin/bash

# Start Docker Compose for PostgreSQL

echo "Starting PostgreSQL for Identity Service"
cd Identity
docker-compose up -d
cd ..

# Wait for PostgreSQL to be ready
sleep 3
echo "PostgreSQL is ready!"

# Apply EF Core migrations 

cd Identity
cd Identity.Data
dotnet ef database update
cd ..

# Run the .NET Solution 

cd Identity
export ASPNETCORE_URLS=https://localhost:7108
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore Identity.sln
dotnet run Identity.sln &

