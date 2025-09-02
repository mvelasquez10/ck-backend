#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Users/API/CK.Rest.Users/CK.Rest.Users.csproj", "src/Services/Users/API/CK.Rest.Users/"]
RUN dotnet restore "src/Services/Users/API/CK.Rest.Users/CK.Rest.Users.csproj"
COPY . .
WORKDIR "/src/src/Services/Users/API/CK.Rest.Users"
COPY "src/Services/Users/API/CK.Rest.Users/appsettings.json" .
RUN dotnet build "CK.Rest.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CK.Rest.Users.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/Repository
RUN chown -R app:app /app/Repository
COPY ["src/Domain/Infrastructure/CK.Repository.SQLite/sqlite/CK.Repository.SQLite.dll", "/app/Repository/sqlite/"]
COPY ["src/Domain/Infrastructure/CK.Repository.SQLite/sqlite/libe_sqlite3.so", "/app/Repository/sqlite/"]
USER app
ENTRYPOINT ["dotnet", "CK.Rest.Users.dll"]
