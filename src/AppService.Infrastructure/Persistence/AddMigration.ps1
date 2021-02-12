param([String]$name)

dotnet ef migrations add $name --project .\AppService.Infrastructure.csproj --framework net5.0