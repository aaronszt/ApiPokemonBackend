FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos csproj y restaurar dependencias
COPY ["Pokemon.Web/Pokemon.Web.csproj", "Pokemon.Web/"]
COPY ["Pokemon.Domain/Pokemon.Domain.csproj", "Pokemon.Domain/"]
COPY ["Pokemon.Infrastructure/Pokemon.Infrastructure.csproj", "Pokemon.Infrastructure/"]
COPY ["Pokemon.Persistence/Pokemon.Persistence.csproj", "Pokemon.Persistence/"]
RUN dotnet restore "Pokemon.Web/Pokemon.Web.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/Pokemon.Web"
RUN dotnet build "Pokemon.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokemon.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokemon.Web.dll"]
