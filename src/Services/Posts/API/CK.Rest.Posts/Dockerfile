#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Posts/API/CK.Rest.Posts/CK.Rest.Posts.csproj", "src/Services/Posts/API/CK.Rest.Posts/"]
RUN dotnet restore "src/Services/Posts/API/CK.Rest.Posts/CK.Rest.Posts.csproj"
COPY . .
WORKDIR "/src/src/Services/Posts/API/CK.Rest.Posts"
RUN dotnet build "CK.Rest.Posts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CK.Rest.Posts.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/Repository
RUN chown -R app:app /app/Repository
COPY ["src/Domain/Infrastructure/CK.Repository.SQLite/sqlite/CK.Repository.SQLite.dll", "/app/Repository/sqlite/"]
COPY ["src/Domain/Infrastructure/CK.Repository.SQLite/sqlite/libe_sqlite3.so", "/app/Repository/sqlite/"]
USER app
ENTRYPOINT ["dotnet", "CK.Rest.Posts.dll"]
