FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj \
	 src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj

COPY src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj \
	 src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj

COPY src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj \
	 src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj

COPY src/StuffyHelper.Authorization.Data/StuffyHelper.Authorization.Data.csproj \
	 src/StuffyHelper.Authorization.Data/StuffyHelper.Authorization.Data.csproj
	 
COPY src/StuffyHelper.Common/StuffyHelper.Common.csproj \
	 src/StuffyHelper.Common/StuffyHelper.Common.csproj
	 
COPY src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj \
	 src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj
	 
COPY src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj \
	 src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

COPY src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj \
	 src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj
	 
COPY src/StuffyHelper.Core/StuffyHelper.Core.csproj \
	 src/StuffyHelper.Core/StuffyHelper.Core.csproj
	 
COPY src/StuffyHelper.Data/StuffyHelper.Data.csproj \
	 src/StuffyHelper.Data/StuffyHelper.Data.csproj
	 
COPY src/StuffyHelper.Contracts/StuffyHelper.Contracts.csproj \
	 src/StuffyHelper.Contracts/StuffyHelper.Contracts.csproj

COPY src/StuffyHelper.Minio/StuffyHelper.Minio.csproj \
	 src/StuffyHelper.Minio/StuffyHelper.Minio.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Api/StuffyHelper.Authorization.Api.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Contracts/StuffyHelper.Authorization.Contracts.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Data/StuffyHelper.Authorization.Data.csproj

RUN dotnet restore src/StuffyHelper.Common/StuffyHelper.Common.csproj

RUN dotnet restore src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj

RUN dotnet restore src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

RUN dotnet restore src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj

RUN dotnet restore src/StuffyHelper.Core/StuffyHelper.Core.csproj

RUN dotnet restore src/StuffyHelper.Data/StuffyHelper.Data.csproj

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

ENV ASPNETCORE_URLS=http://+:7168
EXPOSE 7168

ENTRYPOINT ["dotnet", "StuffyHelper.Authorization.Api.dll"]