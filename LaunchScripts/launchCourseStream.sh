#!/bin/bash

# ANSI escape code for colors
CYAN="\033[36m"
RED="\033[31m"
GREEN="\033[32m"

RESET="\033[0m"

if [ $# -ne 1 ]; then
    echo "Usage: $0 <container-name>"
    exit 1
fi

container_name="$1"

# Start Docker Compose for PostgreSQL
cd ..
cd Identity || exit
echo "Starting Docker Container"
docker-compose up -d

timeout=$((SECONDS + 60))  # Set a timeout of 60 seconds
interval=1

while [ $SECONDS -lt $timeout ]; do
    # Check if PostgreSQL container is running
    if docker ps | grep -q "$container_name"; then
        echo -e "${GREEN} Container is running! ${RESET}"
        break
    else
        # Get current timestamp in [hh:mm:ss] format
        timestamp=$(date "+[%H:%M:%S]")
        echo -e "${CYAN}${timestamp} ${container_name} is not running. Retrying in $interval seconds... ${RESET}"
        sleep $interval
    fi
done

if [ $SECONDS -ge $timeout ]; then
    echo -e "${RED} ${container_name} did not start within a minute. Aborting. ${RESET}"
    exit 1
fi

cd ..
# Apply EF Core migrations 
cd CourseStream/CourseStream.Data || exit
dotnet ef database update
cd ../..

# Run the .NET Solution 
cd CourseStream/CourseStream || exit
export ASPNETCORE_URLS=https://localhost:7237
export ASPNETCORE_ENVIRONMENT=Development
dotnet restore CourseStream.sln
dotnet run CourseStream.sln &
