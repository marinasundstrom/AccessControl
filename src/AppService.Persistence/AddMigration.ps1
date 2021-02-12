param([String]$name)

dotnet ef migrations add $name --project .\AppService.Persistence.csproj --framework netcoreapp2.2