# Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EcoTrack360/EcoTrack360.csproj", "EcoTrack360/"]
RUN dotnet restore "EcoTrack360/EcoTrack360.csproj"

COPY . .
WORKDIR /src/EcoTrack360
RUN dotnet publish "EcoTrack360.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV MONGO_CONN="mongodb://mongo:27017"

ENTRYPOINT ["dotnet", "EcoTrack360.dll"]
