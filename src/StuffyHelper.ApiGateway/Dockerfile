FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY src/StuffyHelper.ApiGateway/StuffyHelper.ApiGateway.csproj \
	 src/StuffyHelper.ApiGateway/StuffyHelper.ApiGateway.csproj
	 
COPY src/StuffyHelper.ApiGateway.Core/StuffyHelper.ApiGateway.Core.csproj \
	 src/StuffyHelper.ApiGateway.Core/StuffyHelper.ApiGateway.Core.csproj

COPY src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj \
	 src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj
 
COPY src/StuffyHelper.Common/StuffyHelper.Common.csproj \
	 src/StuffyHelper.Common/StuffyHelper.Common.csproj
	 
COPY src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj \
	 src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj
	 
COPY src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj \
	 src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

COPY src/StuffyHelper.Contracts/StuffyHelper.Contracts.csproj \
	 src/StuffyHelper.Contracts/StuffyHelper.Contracts.csproj

COPY src/StuffyHelper.Minio/StuffyHelper.Minio.csproj \
	 src/StuffyHelper.Minio/StuffyHelper.Minio.csproj


RUN dotnet restore src/StuffyHelper.ApiGateway.Core/StuffyHelper.ApiGateway.Core.csproj

RUN dotnet restore src/StuffyHelper.ApiGateway/StuffyHelper.ApiGateway.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj

RUN dotnet restore src/StuffyHelper.Common/StuffyHelper.Common.csproj

RUN dotnet restore src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj

RUN dotnet restore src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

RUN dotnet restore src/StuffyHelper.Contracts/StuffyHelper.Contracts.csproj

RUN dotnet restore src/StuffyHelper.Minio/StuffyHelper.Minio.csproj

COPY . .

RUN dotnet build src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
USER 1000

ENV ASPNETCORE_URLS=http://+:7654
EXPOSE 7654

ENTRYPOINT ["dotnet", "StuffyHelper.ApiGateway.dll"]