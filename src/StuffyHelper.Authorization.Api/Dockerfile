FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj", "src/StuffyHelper.Authorization.Api/"]
RUN dotnet restore "src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj"
COPY . .
WORKDIR "/src/src/StuffyHelper.Authorization.Api"
RUN dotnet build "StuffyHelper.Authorization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StuffyHelper.Authorization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StuffyHelper.Authorization.Api.dll"]
