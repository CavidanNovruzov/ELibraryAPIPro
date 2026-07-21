# =============================================================
# Stage 1: Runtime Base (Yüngül və təhlükəsiz mühit)
# =============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


USER app

# =============================================================
# Stage 2: SDK Build (Kompilyasiya mərhələsi)
# =============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Core/ELibraryAPI.Application/ELibraryAPI.Application.csproj", "Core/ELibraryAPI.Application/"]
COPY ["Core/ELibraryAPI.Domain/ELibraryAPI.Domain.csproj", "Core/ELibraryAPI.Domain/"]
COPY ["Infrastructure/ELibraryAPI.Infrastructure/ELibraryAPI.Infrastructure.csproj", "Infrastructure/ELibraryAPI.Infrastructure/"]
COPY ["Infrastructure/ELibraryAPI.Persistance/ELibraryAPI.Persistance.csproj", "Infrastructure/ELibraryAPI.Persistance/"]
COPY ["Presentation/ELibraryAPI.API/ELibraryAPI.API.csproj", "Presentation/ELibraryAPI.API/"]

# Paketlərin bərpası (Restore)
RUN dotnet restore "Presentation/ELibraryAPI.API/ELibraryAPI.API.csproj"

# Bütün layihə kodunu köçürürük
COPY . .

# Presentation qatındakı əsas API layihəsini Release rejimində build edirik
WORKDIR "/src/Presentation/ELibraryAPI.API"
RUN dotnet build "ELibraryAPI.API.csproj" -c Release -o /app/build

# =============================================================
# Stage 3: Publish (DLL-lərin optimallaşdırılmış çıxışı)
# =============================================================
FROM build AS publish
RUN dotnet publish "ELibraryAPI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =============================================================
# Stage 4: Final Image (Canlı mühitdə işləyəcək minimal ölçülü imic)
# =============================================================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENV ASPNETCORE_URLS=http://+:8080

# Yüksək trafikli Libraff miqyası üçün Server Garbage Collector rejimini aktivləşdiririk
ENV COMPlus_gcServer=1

ENTRYPOINT ["dotnet", "ELibraryAPI.API.dll"]
