#!/bin/bash

echo "Starting all services..."

# Navigate to the directory containing the scripts if they're not in the same directory as launch.sh
cd LaunchScripts || exit

# Make sure the scripts are executable
chmod +x launchIdentity.sh
chmod +x launchMaterials.sh
chmod +x launchCourseStream.sh
chmod +x launchCourseTemplate.sh
chmod +x launchfrontend.sh

# Execute each script

echo "Launching Identity Service..."
./launchIdentity.sh identity-postgres-container

echo "Launching Materials Service..."
./launchMaterials.sh materials-postgres-container

echo "Launching Course Stream Service..."
./launchCourseStream.sh courseStream-postgres-container

echo "Launching Course Template Service..."
./launchCourseTemplate.sh courseTemplate-postgres-container

echo "Launching Frontend Service..."
./launchfrontend.sh

echo "All services started successfully."
