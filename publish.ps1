dotnet clean
dotnet restore
dotnet build

Set-Location SystemPropertiesChecker
.\publish.ps1

Set-Location ..

Set-Location SystemPropertiesChecker.Avalonia
.\publish.ps1

Set-Location ..

Set-Location SystemPropertiesChecker.Terminal
.\publish.ps1

Set-Location ..