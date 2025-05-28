# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BookPetGroomingAPI.API/BookPetGroomingAPI.API.csproj", "src/BookPetGroomingAPI.API/"]
RUN dotnet restore "src/BookPetGroomingAPI.API/BookPetGroomingAPI.API.csproj"
COPY . .
RUN ls -la /src
RUN ls -la /src/src/BookPetGroomingAPI.API
WORKDIR /src/src/BookPetGroomingAPI.API
RUN pwd
RUN ls -la
RUN dotnet build BookPetGroomingAPI.API.csproj -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BookPetGroomingAPI.API.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookPetGroomingAPI.API.dll"]