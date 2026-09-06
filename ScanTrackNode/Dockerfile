# Din Dockerfile — fyll i de saknade raderna
# Använd multi-stage build precis som på lektionen

# Stage 1: Bygg
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# TODO: Kopiera projektfilen och kör restore
COPY ["ScanTrackNode.csproj", "./"]
RUN dotnet restore "ScanTrackNode.csproj"

# TODO: Kopiera resten av koden och publicera
COPY . .
RUN dotnet build "./ScanTrackNode.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ScanTrackNode.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# TODO: Kopiera CSV-filen (den behövs i runtime-imagen också)
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