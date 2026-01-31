# This script is used to publish the App for different runtimes.
# It sets the target framework to .NET 10.0 and specifies the runtimes for x64 and ARM64 architectures (Windows and Linux).
$targetFramework = "net10.0"
$runtimes = @("win-x64", "win-arm64", "linux-x64", "linux-arm64")
$appsDirectory = "C:\Apps"
$appName = (Get-Item .).Name
$outputBase = "$appsDirectory\$appName"

# Copy AppLauncher and rename it to the app name (only for Windows)
$appLauncherSource = "$appsDirectory\AppLauncher\x64\AppLauncher.exe"
$appLauncherTarget = "$outputBase\$appName.exe"

foreach ($runtime in $runtimes) {
    if ($runtime.StartsWith("win-")) {
        dotnet publish -c Release -o "$outputBase\$($runtime.Replace('win-', ''))" -r $runtime -f $targetFramework --no-self-contained
    }
    else {
        dotnet publish -c Release -o "$outputBase\$runtime" -r $runtime -f $targetFramework --self-contained
    }
}

if (Test-Path $appLauncherSource) {
    Write-Output "Copying AppLauncher to $appLauncherTarget..."
    Copy-Item -Path $appLauncherSource -Destination $appLauncherTarget -Force
    Write-Output "Launcher ready: $appLauncherTarget"
}
else {
    Write-Warning "AppLauncher not found at $appLauncherSource"
}
