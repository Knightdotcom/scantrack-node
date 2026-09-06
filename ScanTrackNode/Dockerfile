# Din Dockerfile — fyll i de saknade raderna
# Använd multi-stage build precis som på lektionen

# Stage 1: Bygg
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file into ScanTrackNode/ and restore
COPY ["ScanTrackNode/ScanTrackNode.csproj", "ScanTrackNode/"]
RUN dotnet restore "ScanTrackNode/ScanTrackNode.csproj"

# Copy whole repo into image so ../data is available, then build from project folder
COPY . .
WORKDIR /src/ScanTrackNode
RUN dotnet build "ScanTrackNode.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/ScanTrackNode
RUN dotnet publish "ScanTrackNode.csproj" -c $BUILD_CONFIGURATION -o /app/publish
# Bake the CSV into the image for production/ACR builds. Ensure data/cities.csv exists in the repository root build context.
COPY ["data/cities.csv", "/app/publish/data/cities.csv"]

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# TODO: Kopiera det publicerade projektet från build-steget
COPY --from=publish /app/publish .

# Porten appen lyssnar på inuti containern
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# TODO: Starta appen
ENTRYPOINT ["dotnet", "ScanTrackNode.dll"]