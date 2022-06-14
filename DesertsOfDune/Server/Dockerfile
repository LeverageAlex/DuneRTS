FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# fetch resources for the server, so the server and the GameData package and
# copy them into the container
COPY Server/Server.csproj Server/
COPY GameData/GameData.csproj ../GameData/

# reset / restore all server dependencies
RUN dotnet restore Server/Server.csproj

# build the server to get the .dll
COPY . .
WORKDIR /src/Server
RUN dotnet build Server.csproj -c Release -o /app

# publish the server, so create a deployable folder
FROM build AS publish
RUN dotnet publish Server.csproj -c Release -o /app
COPY --from=build /src /app/src

# copy the deployable / executable "build" of the server into the current working directory,
# to launch the built server
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# expose the the port, on which the docker container will be accessable in 
# the host system
EXPOSE 8000

# run the server with the arguments given in the docker compose
CMD ["sh", "-c", "dotnet Server.dll --config-match src/Server/Configuration/ConfigurationFiles/team08.party.json -s src/Server/Configuration/ConfigurationFiles/team08.scenario.json --port ${PORT}"]