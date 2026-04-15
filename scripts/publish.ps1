# Clean, restore, and build the solution
dotnet clean
dotnet restore
dotnet build

$configPath = Join-Path $PSScriptRoot "publish.json"
$publishConfigs = Get-Content $configPath | ConvertFrom-Json

foreach ($config in $publishConfigs) {
    $projectName = $config.project
    $targetFramework = $config.targetFramework
    $runtimes = $config.runtimes
    $selfContained = $config.selfContained
    $withAppLauncher = $config.withAppLauncher
    
    $appsDirectory = "C:\Apps"
    $outputBase = "$appsDirectory\$projectName"
    $projectPath = Join-Path $PSScriptRoot "..\src\$projectName\$projectName.csproj"
    
    Write-Output "Start publishing '$projectName'..."
    
    foreach ($runtime in $runtimes) {
        if ($runtime.StartsWith("win-")) {
            $outputPath = "$outputBase\$($runtime.Replace('win-', ''))"
            $scParam = if ($selfContained) { "--self-contained" } else { "--no-self-contained" }
            dotnet publish $projectPath -c Release -o $outputPath -r $runtime -f $targetFramework $scParam
        }
        else {
            $outputPath = "$outputBase\$runtime"
            dotnet publish $projectPath -c Release -o $outputPath -r $runtime -f $targetFramework --self-contained
        }
    }

    if ($withAppLauncher) {
        # Copy AppLauncher and rename it to the app name (only for Windows)
        $appLauncherSource = "$appsDirectory\AppLauncher\x64\AppLauncher.exe"
        $appLauncherTarget = "$outputBase\$projectName.exe"
        
        if (Test-Path $appLauncherSource) {
            Write-Output "Copying AppLauncher to $appLauncherTarget..."
            Copy-Item -Path $appLauncherSource -Destination $appLauncherTarget -Force
            Write-Output "Launcher ready: $appLauncherTarget"
        }
        else {
            Write-Warning "AppLauncher not found at $appLauncherSource"
        }
    }
}
