Instructions on how to start the project 
This is a sample app built using the lifebook framework

how to run Schoolbook:

# System requirements 
* Docker Installer
* .net core 3.0+

```

cd ./lifebook.infastructure
docker-compose up
cd ../SchoolBook/SchoolBookApp
dotnet restore lifebook.SchoolBookApp.csproj --source ../../lifebook-nuget-feed 
dotnet restore lifebook.SchoolBookApp.csproj --source ../../lifebook-nuget-feed 
dotnet run lifebook.SchoolBookApp.csproj
```
