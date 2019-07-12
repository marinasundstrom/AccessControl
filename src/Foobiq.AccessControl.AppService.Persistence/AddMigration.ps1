param([String]$name)

dotnet ef migrations add $name --project .\Foobiq.AccessControl.AppService.Persistence.csproj --framework netcoreapp2.2