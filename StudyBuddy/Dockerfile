# 1. Get the .NET 10.0 Runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Get the .NET 10.0 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything into the container and build it
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# 3. Publish the compiled code
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# 4. Final stage - copy files and run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyBuddy.dll"]