echo "Starting Frontend"

cd ..
cd frontend

# Check if npm is installed
if ! command -v npm &> /dev/null; then
    echo "npm could not be found, please install it."
    exit 1
fi

# Optionally, install dependencies if they are not already installed
echo "Installing any missing dependencies..."
npm install

# Start the frontend in the background and redirect output to a log file
echo "Launching Frontend Service..."
npm start > frontend.log 2>&1 &

echo "Frontend Service started."