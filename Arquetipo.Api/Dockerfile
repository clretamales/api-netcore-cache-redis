# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) Copiar solo el archivo de proyecto y restaurar dependencias
COPY ["Arquetipo.Api.csproj", "./"]
RUN dotnet restore "Arquetipo.Api.csproj"

# 2) Copiar todo el código y compilar
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# 3) Copiar artefactos de publicación
COPY --from=build /app/publish .

# 4) Puerto expuesto y punto de entrada
EXPOSE 80
ENTRYPOINT ["dotnet", "Arquetipo.Api.dll"]