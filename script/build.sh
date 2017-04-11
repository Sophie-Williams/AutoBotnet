#!/usr/bin/env bash

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

# Dotnet info

dotnet --version

echo "Restoring packages..."
# Restore packages
dotnet restore src/server/Speercs.Server.sln

echo "Building project..."

# Build
dotnet build -c Release src/server/Speercs.Server

echo "Running tests..."

echo "Publishing project..."

# Publish
dotnet publish -c Release src/server/Speercs.Server