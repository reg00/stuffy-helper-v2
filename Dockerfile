FROM mcr.microsoft.com/dotnet/sdk:6.0.202-alpine3.14 AS build

WORKDIR /src

COPY src/StuffyHelper.Hosting/StuffyHelper.Hosting.csproj \
     src/StuffyHelper.Hosting/StuffyHelper.Hosting.csproj

COPY src/StuffyHelper.Api/StuffyHelper.Api.csproj \
     src/StuffyHelper.Api/StuffyHelper.Api.csproj

COPY src/StuffyHelper.Core/StuffyHelper.Core.csproj \
     src/StuffyHelper.Core/StuffyHelper.Core.csproj

COPY src/StuffyHelper.EntityFrameworkCore/StuffyHelper.EntityFrameworkCore.csproj \
     src/StuffyHelper.EntityFrameworkCore/StuffyHelper.EntityFrameworkCore.csproj

COPY src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj \
     src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj
	 
COPY src/StuffyHelper.Authorization.EntityFrameworkCore/StuffyHelper.Authorization.EntityFrameworkCore.csproj \
     src/StuffyHelper.Authorization.EntityFrameworkCore/StuffyHelper.Authorization.EntityFrameworkCore.csproj

COPY src/StuffyHelper.Minio/StuffyHelper.Minio.csproj \
     src/StuffyHelper.Minio/StuffyHelper.Minio.csproj

RUN dotnet restore src/StuffyHelper.Hosting/StuffyHelper.Hosting.csproj

RUN dotnet restore src/StuffyHelper.Api/StuffyHelper.Api.csproj

RUN dotnet restore src/StuffyHelper.Core/StuffyHelper.Core.csproj

RUN dotnet restore src/StuffyHelper.EntityFrameworkCore/StuffyHelper.EntityFrameworkCore.csproj

RUN dotnet restore src/StuffyHelper.Authorization.Core/StuffyHelper.Authorization.Core.csproj

RUN dotnet restore src/StuffyHelper.Authorization.EntityFrameworkCore/StuffyHelper.Authorization.EntityFrameworkCore.csproj

RUN dotnet restore src/StuffyHelper.Minio/StuffyHelper.Minio.csproj

COPY ./ ./

RUN dotnet build "src/StuffyHelper.Hosting/StuffyHelper.Hosting.csproj" --configuration Release
RUN dotnet publish "src/StuffyHelper.Hosting/StuffyHelper.Hosting.csproj" -c Release -o "/.build" --no-build

FROM mcr.microsoft.com/dotnet/aspnet:6.0.2-alpine3.14 AS runtime

RUN set -x && \
    apk add --no-cache --update musl musl-utils musl-locales tzdata icu-libs && \
    cp /usr/share/zoneinfo/Europe/Moscow /etc/localtime && \
    echo "Europe/Moscow" > /etc/timezone && \
    echo 'export LC_ALL=ru_RU.UTF-8' >> /etc/profile.d/locale.sh && \
    sed -i 's|LANG=C.UTF-8|LANG=ru_RU.UTF-8|' /etc/profile.d/locale.sh && \
    addgroup nonroot && \
    adduser -S -D -H -s /sbin/nologin -G nonroot -g nonroot nonroot 

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV TZ Europe/Moscow
ENV LANG ru_RU.UTF-8
ENV LANGUAGE ru_RU.UTF-8
ENV LC_ALL ru_RU.UTF-8

WORKDIR /app
COPY --from=build .build .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "StuffyHelper.Hosting.dll"]