dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\x64" -r win-x64 -f net9.0 --no-self-contained
dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\arm64" -r win-arm64 -f net9.0 --no-self-contained

dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\linux-x64" -r linux-x64 -f net9.0 --self-contained
dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\linux-arm64" -r linux-arm64 -f net9.0 --self-contained
