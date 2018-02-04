#!/usr/bin/env bash

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

# dotnet info
echo "Building using .NET Core SDK $(dotnet --version)"

echo "Running dotnet restore"
# Restore packages
dotnet restore src/server/Speercs.Server.sln

echo "Running dotnet build [Release]"

# Build
dotnet build -c Release src/server/Speercs.Server

echo "Running tests"

echo "Running dotnet publish [Release]"

# Publish
dotnet publish -c Release src/server/Speercs.Server