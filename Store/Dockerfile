FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app/src

# copy in all the projects
COPY Store.WebApp/*.csproj Store.WebApp/
COPY Store.Test/*.csproj Store.Test/
COPY Store.DataModel/*.csproj Store.DataModel/
COPY Library/*.csproj Library/
COPY *.sln ./

# restore projects
RUN dotnet restore


COPY . ./

RUN dotnet publish Store.WebApp -c Release -o ../publish

# PACKAGE ASSEMBLIES

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/publish ./

CMD dotnet Store.WebApp.dll
