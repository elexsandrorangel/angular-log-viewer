#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LogViewer/LogViewer.csproj", "LogViewer/"]
COPY ["LogViewer.Infrastructure/LogViewer.Infrastructure.csproj", "LogViewer.Infrastructure/"]
COPY ["LogViewer.Repository/LogViewer.Repository.csproj", "LogViewer.Repository/"]
COPY ["LogViewer.Business/LogViewer.Business.csproj", "LogViewer.Business/"]
COPY ["LogViewer.Models/LogViewer.Models.csproj", "LogViewer.Models/"]
RUN dotnet restore "LogViewer/LogViewer.csproj"
COPY . .
WORKDIR "/src/LogViewer"
RUN dotnet build "LogViewer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogViewer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogViewer.dll"]