FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY src/StuffyHelper.Common/StuffyHelper.Common.csproj \
	 src/StuffyHelper.Common/StuffyHelper.Common.csproj
	 
COPY src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj \
	 src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj
	 
COPY src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj \
	 src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

COPY src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj \
	 src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj
	 
COPY src/StuffyHelper.EmailService.Core/StuffyHelper.EmailService.Core.csproj \
	 src/StuffyHelper.EmailService.Core/StuffyHelper.EmailService.Core.csproj

COPY src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj \
	 src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj

RUN dotnet restore src/StuffyHelper.Common/StuffyHelper.Common.csproj

RUN dotnet restore src/StuffyHelper.Common.Client/StuffyHelper.Common.Client.csproj

RUN dotnet restore src/StuffyHelper.Common.Contracts/StuffyHelper.Common.Contracts.csproj

RUN dotnet restore src/StuffyHelper.EmailService.Contracts/StuffyHelper.EmailService.Contracts.csproj

RUN dotnet restore src/StuffyHelper.EmailService.Core/StuffyHelper.EmailService.Core.csproj

RUN dotnet restore src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj

COPY . .

RUN dotnet build src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish src/StuffyHelper.EmailService.Api/StuffyHelper.EmailService.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
USER 1000

ENV ASPNETCORE_URLS=http://+:7755
EXPOSE 7755

ENTRYPOINT ["dotnet", "StuffyHelper.EmailService.Api.dll"]