# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file
COPY HotelManagement.WebApi/HotelManagement.WebApi.csproj HotelManagement.WebApi/

# Restore dependencies
RUN dotnet restore HotelManagement.WebApi/HotelManagement.WebApi.csproj

# Copy everything else
COPY . .

# Publish the app
RUN dotnet publish HotelManagement.WebApi/HotelManagement.WebApi.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HotelManagement.WebApi.dll"]
