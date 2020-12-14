FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base  
WORKDIR /app  
EXPOSE 80  
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  
COPY Restpirators.Gateway/*.csproj ./Restpirators.Gateway
RUN dotnet restore "Restpirators.Gateway"
COPY . .  
WORKDIR /src/Restpirators.Gateway
RUN dotnet build Restpirators.Gateway.csproj -c Release -o /app/build 

FROM build AS publish  
RUN dotnet publish Restpirators.Gateway.csproj -c Release -o /app/publish 

FROM base AS final  
WORKDIR /app  
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "Restpirators.Gateway.dll"]  