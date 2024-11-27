# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

RUN mkdir -p /app/volume/image && \
    chmod -R 775 /app/volume/image

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["vigo.Admin/vigo.Admin.csproj", "vigo.Admin/"]
COPY ["vigo.Domain/vigo.Domain.csproj", "vigo.Domain/"]
COPY ["vigo.Infrastructure/vigo.Infrastructure.csproj", "vigo.Infrastructure/"]
COPY ["vigo.Service/vigo.Service.csproj", "vigo.Service/"]
RUN dotnet restore "./vigo.Admin/vigo.Admin.csproj"
COPY . .
WORKDIR "/src/vigo.Admin"
RUN dotnet build "./vigo.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./vigo.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["vigo.Admin/image", "/app/volume/image"]
ENTRYPOINT ["dotnet", "vigo.Admin.dll"]