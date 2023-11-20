# Use the .NET 7 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

# Copy the CSPROJ files and restore any dependencies (via NUGET)
COPY Identity/*.csproj ./Identity/
COPY Identity.Core/*.csproj ./Identity.Core/
COPY Identity.Data/*.csproj ./Identity.Data/

RUN dotnet restore Identity/Identity.csproj

# Copy the project files
COPY Identity/ ./Identity/
COPY Identity.Core/ ./Identity.Core/
COPY Identity.Data/ ./Identity.Data/

# Copy the certificate into the Docker image
COPY Identity/academichub.pfx /app/academichub.pfx

RUN chmod 644 /app/academichub.pfx

# Build and publish the release
RUN dotnet publish Identity/Identity.csproj -c Release -o /app/out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 7108


# Set environment variables
# ENV ASPNETCORE_URLS=https://+:7108
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Start the application
ENTRYPOINT ["dotnet", "Identity.dll"]
