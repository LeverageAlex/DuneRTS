FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# fetch resources for the ai client, so the ai client and the GameData package and
# copy them into the container
COPY AIClient/AIClient.csproj AIClient/
COPY GameData/GameData.csproj ../GameData/

# reset / restore all server dependencies
RUN dotnet restore AIClient/AIClient.csproj

# build the ai client to get the .dll
COPY . .
WORKDIR /src/AIClient
RUN dotnet build AIClient.csproj -c Release -o /app

# publish the ai client, so create a deployable folder
FROM build AS publish
RUN dotnet publish AIClient.csproj -c Release -o /app
COPY --from=build /src /app/src

# copy the deployable / executable "build" of the ai client into the current working directory,
# to launch the built ai client
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# expose the the port, on which the docker container will be accessable in 
# the host system
EXPOSE ${PORT}

# run the ai client with the arguments given in the docker compose
CMD ["sh", "-c", "dotnet AIClient.dll -a ${SERVER_ADDRESS} -p ${PORT} -n ${NAME}"]
