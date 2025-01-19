FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TrackingApi.csproj", "./"]
RUN dotnet restore "TrackingApi.csproj"
COPY . .
RUN dotnet build "TrackingApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "TrackingApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["appsettings.json", "appsettings.json"]
COPY ["appsettings.Development.json", "appsettings.Development.json"]
ENTRYPOINT ["dotnet", "TrackingApi.dll"]
