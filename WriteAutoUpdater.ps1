$productName = "SystemPropertiesChecker"
$releaseUrl = "https://github.com/evilbaschdi/"+$productName+"/releases"
$url = $releaseUrl + "/download/4.1.112/ServiceBusExplorer-4.1.112.zip"

(Get-Item SystemPropertiesChecker.exe).VersionInfo.FileVersion