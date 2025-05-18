# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo y restaurar paquetes
COPY . .
RUN dotnet restore "ApiLaBodeguita.csproj"

# Publicar el proyecto
RUN dotnet publish "ApiLaBodeguita.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiLaBodeguita.dll"]
