# ====== STAGE 1: BUILD ======
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY SoundFy.sln ./
COPY Adm/Adm.csproj Adm/
COPY Business/Business.csproj Business/
COPY Data/Data.csproj Data/
COPY WebApp/WebApp.csproj WebApp/
RUN dotnet restore SoundFy.sln

COPY . ./
RUN dotnet publish Adm/Adm.csproj -c Release -o /app/adm
RUN dotnet publish WebApp/WebApp.csproj -c Release -o /app/webapp

# ====== STAGE 2: RUNTIME ======
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app /app
