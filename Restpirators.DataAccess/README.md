#Create and apply migrations

##Install dotnet-ef tool (Do only once per machine)
dotnet tool install --global dotnet-ef --version 3.1.10

##Set DataAccess

##Create new migration script
dotnet ef migrations add <Name of your migration> --project .\Restpirators.DataAccess

##Apply migrations to specified DB