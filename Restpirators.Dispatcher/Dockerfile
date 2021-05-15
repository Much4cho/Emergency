#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Restpirators.Dispatcher/Restpirators.Dispatcher.csproj", "Restpirators.Dispatcher/"]
COPY ["Restpirators.Repository/Restpirators.Repository.csproj", "Restpirators.Repository/"]
COPY ["Restpirators.DataAccess/Restpirators.DataAccess.csproj", "Restpirators.DataAccess/"]
COPY ["Restpirators.Common/Restpirators.Common.csproj", "Restpirators.Common/"]
COPY ["Restpirator.Messaging/Restpirator.Messaging.csproj", "Restpirator.Messaging/"]
RUN dotnet restore "Restpirators.Dispatcher/Restpirators.Dispatcher.csproj"
COPY . .
WORKDIR "/src/Restpirators.Dispatcher"
RUN dotnet build "Restpirators.Dispatcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Restpirators.Dispatcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restpirators.Dispatcher.dll"]