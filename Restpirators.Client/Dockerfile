#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Restpirators.Client/Restpirators.Client.csproj", "Restpirators.Client/"]
COPY ["Restpirators.Common/Restpirators.Common.csproj", "Restpirators.Common/"]
RUN dotnet restore "Restpirators.Client/Restpirators.Client.csproj"
COPY . .
WORKDIR "/src/Restpirators.Client"
RUN dotnet build "Restpirators.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Restpirators.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restpirators.Client.dll"]