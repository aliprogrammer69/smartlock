#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UI/SmartLock.UI.RestApi/SmartLock.UI.RestApi.csproj", "UI/SmartLock.UI.RestApi/"]
RUN dotnet restore "UI/SmartLock.UI.RestApi/SmartLock.UI.RestApi.csproj"
COPY . .
WORKDIR "/src/UI/SmartLock.UI.RestApi"
RUN dotnet build "SmartLock.UI.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartLock.UI.RestApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartLock.UI.RestApi.dll"]