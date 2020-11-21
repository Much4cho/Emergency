#Create and apply migrations

##Install dotnet-ef tool (Do only once per machine)
dotnet tool install --global dotnet-ef --version 3.1.10

##Set DataAccess as a startup project

##Create new migration script
dotnet ef migrations add <Name of your migration> --project .\Restpirators.Repository

##Apply migrations to specified DB
dotnet ef database update --project .\Restpirators.Repository


#You can also use Visual Studio only Commands:
add-migration CreateSchoolDB
update-database –verbose