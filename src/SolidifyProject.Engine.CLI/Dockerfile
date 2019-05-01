# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY SolidifyProject.Engine.Cli/.             ./SolidifyProject.Engine.CLI/.
COPY SolidifyProject.Engine.Configuration/.   ./SolidifyProject.Engine.Configuration/.
COPY SolidifyProject.Engine.Infrastructure/.  ./SolidifyProject.Engine.Infrastructure/.
COPY SolidifyProject.Engine.Services/.        ./SolidifyProject.Engine.Services/.
COPY SolidifyProject.Engine.Utils/.           ./SolidifyProject.Engine.Utils/.

RUN dotnet publish SolidifyProject.Engine.CLI/SolidifyProject.Engine.CLI.csproj -c Release -o out


# Runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "SolidifyProject.Engine.CLI.dll"]