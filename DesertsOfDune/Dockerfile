FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY Server/Server.csproj Server/
COPY GameData/GameData.csproj ../GameData/

RUN dotnet restore Server/Server.csproj

COPY . .
WORKDIR /src/Server
RUN dotnet build Server.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Server.csproj -c Release -o /app
COPY --from=build /src /app/src

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

EXPOSE 8000

ENTRYPOINT ["dotnet", "Server.dll"]
CMD ["--config-match","src/Server/Configuration/ConfigurationFiles/team08.party.json","-s","src/Server/Configuration/ConfigurationFiles/team08.scenario.json","--port","10101"]