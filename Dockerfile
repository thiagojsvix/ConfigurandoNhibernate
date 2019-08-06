FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["src/ConsoleApp/ConsoleApp.csproj", "src/ConsoleApp/"]
RUN dotnet restore "src/ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/src/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]
