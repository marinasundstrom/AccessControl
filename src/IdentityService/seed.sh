CS="Server=localhost,1433;User Id=sa;Password=P@ssw0rd"

dotnet run --project ./IdentityService/IdentityService.csproj -- --seed --connection-string "$CS;Database=IdentityServer"