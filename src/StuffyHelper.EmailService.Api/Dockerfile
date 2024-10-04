FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj", "src/StuffyHelper.EmailService.Api/"]
RUN dotnet restore "src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj"
COPY . .
WORKDIR "/src/src/StuffyHelper.EmailService.Api"
RUN dotnet build "StuffyHelper.EmailService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StuffyHelper.EmailService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StuffyHelper.EmailService.Api.dll"]
