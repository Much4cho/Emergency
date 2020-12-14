FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base  
WORKDIR /app  
EXPOSE 80  
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  
COPY Restpirators.Analyzer/*.csproj ./Restpirators.Analyzer
COPY Restpirators.Common ./Restpirators.Common 
RUN dotnet restore "Restpirators.Analyzer"
COPY . .  
WORKDIR /src/Restpirators.Analyzer
RUN dotnet build Restpirators.Analyzer.csproj -c Release -o /app/build 

FROM build AS publish  
RUN dotnet publish Restpirators.Analyzer.csproj -c Release -o /app/publish 

FROM base AS final  
WORKDIR /app  
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "Restpirators.Analyzer.dll"]  