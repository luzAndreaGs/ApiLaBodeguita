# Usa el SDK de .NET como imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiLaBodeguita.csproj", "./"]
RUN dotnet restore "./ApiLaBodeguita.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ApiLaBodeguita.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiLaBodeguita.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiLaBodeguita.dll"]
