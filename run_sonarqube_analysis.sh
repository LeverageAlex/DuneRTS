#!/bin/bash
dotnet sonarscanner begin /k:"Deserts-of-Dune" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="f60a177be5f525fd247015e8ae2bafe28ac34c73"
dotnet build DesertsOfDune/DesertsOfDune.sln
dotnet sonarscanner end /d:sonar.login="f60a177be5f525fd247015e8ae2bafe28ac34c73"