# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY HotelManagement.sln .
COPY HotelManagement.WebApi/HotelManagement.WebApi.csproj HotelManagement.WebApi/

# Restore dependencies
RUN dotnet restore HotelManagement.sln

# Copy all source files
COPY . .

# Build the project
RUN dotnet publish HotelManagement.WebApi/HotelManagement.WebApi.csproj -c Release -o /app/publish

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Start the API
ENTRYPOINT ["dotnet", "HotelManagement.WebApi.dll"]
