param([String]$name)

dotnet ef migrations add $name --project .\AccessControl.AppService.Persistence.csproj --framework netcoreapp2.2