# .NET 6 Web API
Some of my experiences implementing an API using .NET 6

### SQL Server 2019 Docker

$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=RedHat2022@" -p 1433:1433 --name sqlserver --hostname sql1 -d mcr.microsoft.com/mssql/server:2019-latest