#!/bin/bash

# Check if enough arguments are provided
if [ $# -lt 3 ]; then
  echo "Usage: $0 <coverage-report-file> <module-name> <coverage-type> <threshold>"
  exit 1
fi

# Assign the arguments to variables
COBERTURA_REPORT=$1
MODULE_NAME=$2
COVERAGE_TYPE=$3
THRESHOLD=$4

# Check if the coverage report file exists
if [ ! -f "$COBERTURA_REPORT" ]; then
  echo "Coverage report file not found: $COBERTURA_REPORT"
  exit 1
fi

# Extract the specified coverage percentage
COVERAGE_PERCENTAGE=$(grep "$MODULE_NAME" "$COBERTURA_REPORT" | grep "$COVERAGE_TYPE-rate" | head -n 1 | sed -E "s/.*$COVERAGE_TYPE-rate=\"([0-9.]+)\".*/\1/" | awk '{print $1 * 100}')

# Check if coverage data is found
if [ -z "$COVERAGE_PERCENTAGE" ]; then
  echo "No $COVERAGE_TYPE coverage data for $MODULE_NAME found."
  exit 1
fi

# Compare and exit accordingly
if (( $(echo "$COVERAGE_PERCENTAGE < $THRESHOLD" | bc -l) )); then
  echo "$MODULE_NAME $COVERAGE_TYPE coverage is below $THRESHOLD%. It is $COVERAGE_PERCENTAGE%"
  exit 1
else
  echo "$MODULE_NAME $COVERAGE_TYPE coverage is above $THRESHOLD%. It is $COVERAGE_PERCENTAGE%"
fi
