
# STAGE 1: Base Runtime Environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# High-Performance Server Garbage Collection (Enterprise/High-load APIs üçün)
ENV DOTNET_gcServer=1

# Healthcheck üçün 'curl' paketinin minimal quraşdırılması
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

# Təhlükəsizlik: Root hüquqlarından imtina edib non-root istifadəçiyə keçid
USER $APP_UID

# Docker Healthcheck mexanizmi 
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1


# STAGE 2: Build Stage (Nuget Cache Optimization)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Docker Build Cache optimization: Əvvəlcə yalnız .csproj fayllarını kopyalayırıq.
# Bu addım sayəsində kod dəyişsə belə, 'dotnet restore' yenidən çalışmır, vaxta qənaət edilir.
COPY ["Presentation/ELibraryAPI.API/ELibraryAPI.API.csproj", "Presentation/ELibraryAPI.API/"]
COPY ["Core/ELibraryAPI.Application/ELibraryAPI.Application.csproj", "Core/ELibraryAPI.Application/"]
COPY ["Core/ELibraryAPI.Domain/ELibraryAPI.Domain.csproj", "Core/ELibraryAPI.Domain/"]
COPY ["Infrastructure/ELibraryAPI.Infrastructure/ELibraryAPI.Infrastructure.csproj", "Infrastructure/ELibraryAPI.Infrastructure/"]
COPY ["Infrastructure/ELibraryAPI.Persistance/ELibraryAPI.Persistance.csproj", "Infrastructure/ELibraryAPI.Persistance/"]

RUN dotnet restore "Presentation/ELibraryAPI.API/ELibraryAPI.API.csproj"

# Bütün layihə kodunu kopyalayırıq və build edirik
COPY . .
WORKDIR "/src/Presentation/ELibraryAPI.API"
RUN dotnet build "ELibraryAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# STAGE 3: Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ELibraryAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# STAGE 4: Final Runtime Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ELibraryAPI.API.dll"]