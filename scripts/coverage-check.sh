#!/bin/bash

# Define the threshold
THRESHOLD=95

# Find the coverage report
COBERTURA_REPORT=$(find . -name "coverage.cobertura.xml" | head -n 1)

if [ -z "$COBERTURA_REPORT" ]; then
  echo "No coverage report found."
  exit 1
fi

# Extract Identity.Core branch coverage percentage
IDENTITY_CORE_BRANCH_COVERAGE=$(grep 'Identity.Core' "$COBERTURA_REPORT" | grep 'branch-rate' | sed -E 's/.*branch-rate="([0-9.]+)".*/\1/' | awk '{print $1 * 100}')

# Check if Identity.Core branch coverage is found
if [ -z "$IDENTITY_CORE_BRANCH_COVERAGE" ]; then
  echo "No branch coverage data for Identity.Core found."
  exit 1
fi

# Compare and exit accordingly
if (( $(echo "$IDENTITY_CORE_BRANCH_COVERAGE < $THRESHOLD" | bc -l) )); then
  echo "Identity.Core branch coverage is below $THRESHOLD%. It is $IDENTITY_CORE_BRANCH_COVERAGE%"
  exit 1
else
  echo "Identity.Core branch coverage is above $THRESHOLD%. It is $IDENTITY_CORE_BRANCH_COVERAGE%"
fi
