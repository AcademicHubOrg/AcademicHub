#!/bin/bash

# Define the threshold
THRESHOLD=80

# Find the coverage report
# Assuming there's only one report matching the pattern at a time
COBERTURA_REPORT=$(find . -name "coverage.cobertura.xml" | head -n 1)

if [ -z "$COBERTURA_REPORT" ]; then
  echo "No coverage report found."
  exit 1
fi

# Extract coverage percentage
COVERAGE_PERCENTAGE=$(grep 'line-rate' "$COBERTURA_REPORT" | sed -E 's/.*line-rate="([0-9.]+)".*/\1/' | awk '{print $1 * 100}')

# Compare and exit accordingly
if (( $(echo "$COVERAGE_PERCENTAGE < $THRESHOLD" | bc -l) )); then
  echo "Code coverage is below $THRESHOLD%. It is $COVERAGE_PERCENTAGE%"
  exit 1
else
  echo "Code coverage is above $THRESHOLD%. It is $COVERAGE_PERCENTAGE%"
fi
