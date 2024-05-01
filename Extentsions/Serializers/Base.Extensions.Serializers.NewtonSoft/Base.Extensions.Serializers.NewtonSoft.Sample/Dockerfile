#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Base.Extensions.Serializers.NewtonSoft.Sample/Base.Extensions.Serializers.NewtonSoft.Sample.csproj", "Base.Extensions.Serializers.NewtonSoft.Sample/"]
RUN dotnet restore "./Base.Extensions.Serializers.NewtonSoft.Sample/Base.Extensions.Serializers.NewtonSoft.Sample.csproj"
COPY . .
WORKDIR "/src/Base.Extensions.Serializers.NewtonSoft.Sample"
RUN dotnet build "./Base.Extensions.Serializers.NewtonSoft.Sample.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Base.Extensions.Serializers.NewtonSoft.Sample.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Base.Extensions.Serializers.NewtonSoft.Sample.dll"]