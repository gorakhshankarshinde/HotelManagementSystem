# Use .NET 8 SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore
COPY *.csproj ./
RUN dotnet restore

# Copy rest of the code and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish ./

# Start the application
ENTRYPOINT ["dotnet", "HotelManagement.WebApi.dll"]
