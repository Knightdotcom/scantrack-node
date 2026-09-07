# Multi-stage build för ScanTrackNode
# Byggs från repo-roten:
#   docker build -f ScanTrackNode/Dockerfile -t scantrack-node .

# Stage 1: Bygg
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiera ENDAST projektfilen först och kör restore.
# Detta ger layer-caching: så länge csproj:en är oförändrad
# återanvänds restore-lagret även när källkoden ändras.
COPY ["ScanTrackNode/ScanTrackNode.csproj", "ScanTrackNode/"]
RUN dotnet restore "ScanTrackNode/ScanTrackNode.csproj"

# Kopiera resten av koden (inklusive data/cities.csv i repo-roten)
COPY . .

WORKDIR /src/ScanTrackNode
RUN dotnet publish "ScanTrackNode.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Stage 2: Runtime
# Innehåller bara ASP.NET-runtime, inte hela SDK:t — mindre och säkrare image.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Porten appen lyssnar på inuti containern
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ScanTrackNode.dll"]
