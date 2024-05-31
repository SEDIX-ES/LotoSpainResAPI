FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia el proyecto y restaura las dependencias
COPY *.csproj./
RUN dotnet restore

# Copia el resto de los archivos del proyecto
COPY../
RUN dotnet publish -c Release -o out

# Usa una imagen de runtime.NET para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out.
ENTRYPOINT ["dotnet", "LotoSpainAPI.dll"]