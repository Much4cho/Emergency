FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base  
WORKDIR /app  
EXPOSE 80  
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  
COPY Restpirators.Auth/*.csproj ./Restpirators.Auth
RUN dotnet restore "Restpirators.Auth"
COPY . .  
WORKDIR /src/Restpirators.Auth
RUN dotnet build Restpirators.Auth.csproj -c Release -o /app/build 

FROM build AS publish  
RUN dotnet publish Restpirators.Auth.csproj -c Release -o /app/publish 

FROM base AS final  
WORKDIR /app  
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "Restpirators.Auth.dll"]  