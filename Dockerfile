# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0.418 AS build
WORKDIR /src

COPY ["StudyFlow/StudyFlow.csproj", "StudyFlow/"]
RUN dotnet restore "StudyFlow/StudyFlow.csproj"

COPY . .
WORKDIR "/src/StudyFlow"
RUN dotnet build -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyFlow.dll"]