FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./

# Run CI build script
RUN ./script/build.sh

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/src/server/Speercs.Server/bin/Release/netcoreapp2.0/publish/ .
ENTRYPOINT ["dotnet", "Speercs.Server.dll"]
