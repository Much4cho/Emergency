FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base  
WORKDIR /app  
EXPOSE 80  
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  
COPY Restpirators.Dispatcher/*.csproj ./Restpirators.Dispatcher
COPY Restpirators.Common ./Restpirators.Common 
COPY Restpirator.Messaging ./Restpirator.Messaging
COPY Restpirators.Repository ./Restpirators.Repository
RUN dotnet restore "Restpirators.Dispatcher"
COPY . .  
WORKDIR /src/Restpirators.Dispatcher
RUN dotnet build Restpirators.Dispatcher.csproj -c Release -o /app/build 

FROM build AS publish  
RUN dotnet publish Restpirators.Dispatcher.csproj -c Release -o /app/publish 

FROM base AS final  
WORKDIR /app  
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "Restpirators.Dispatcher.dll"]  