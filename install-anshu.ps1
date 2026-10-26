# Anshu Installer - Run from Terminal
# Usage: 
#   powershell -ExecutionPolicy Bypass -File install-anshu.ps1
# Or copy-paste the one-liner from README

$ErrorActionPreference = "Stop"

Write-Host "=== Anshu Desktop Assistant Installer ===" -ForegroundColor Cyan

# Desktop path
$desktop = [Environment]::GetFolderPath("Desktop")
$installDir = Join-Path $desktop "Anshu"

# Create folder
if (-not (Test-Path $installDir)) {
    New-Item -ItemType Directory -Path $installDir | Out-Null
    Write-Host "Created folder: $installDir" -ForegroundColor Green
}

# Try to download latest release from GitHub
$repo = "raj8077-77/Mate-Engine"
$apiUrl = "https://api.github.com/repos/$repo/releases/latest"

try {
    Write-Host "Checking for latest Anshu release..." -ForegroundColor Yellow
    $release = Invoke-RestMethod -Uri $apiUrl -Headers @{ "User-Agent" = "Anshu-Installer" }
    
    $zipAsset = $release.assets | Where-Object { $_.name -match "\.zip$" -and $_.name -notmatch "source" } | Select-Object -First 1
    
    if ($zipAsset) {
        $zipPath = Join-Path $env:TEMP "Anshu-latest.zip"
        Write-Host "Downloading: $($zipAsset.name)" -ForegroundColor Yellow
        Invoke-WebRequest -Uri $zipAsset.browser_download_url -OutFile $zipPath
        
        Write-Host "Extracting to Desktop\Anshu..." -ForegroundColor Yellow
        Expand-Archive -Path $zipPath -DestinationPath $installDir -Force
        Remove-Item $zipPath -Force
        
        Write-Host "Install complete!" -ForegroundColor Green
        Write-Host "Anshu is ready at: $installDir" -ForegroundColor Green
        Write-Host "Look for MateEngineX.exe or Anshu.exe inside and run it." -ForegroundColor Cyan
    } else {
        Write-Host "No ready-to-go ZIP found in latest release." -ForegroundColor Red
        Write-Host "You need to build the EXE in Unity first and upload it as a Release asset." -ForegroundColor Yellow
    }
} catch {
    Write-Host "Could not download release (maybe no release exists yet)." -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor DarkRed
    Write-Host "" 
    Write-Host "Manual way:" -ForegroundColor Yellow
    Write-Host "1. Build EXE in Unity" -ForegroundColor White
    Write-Host "2. Copy the built folder to Desktop\Anshu" -ForegroundColor White
}

# Create Desktop shortcut if EXE found
$exe = Get-ChildItem -Path $installDir -Recurse -Filter "*.exe" | Where-Object { $_.Name -match "MateEngine|Anshu" } | Select-Object -First 1
if ($exe) {
    $shortcutPath = Join-Path $desktop "Anshu.lnk"
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut($shortcutPath)
    $Shortcut.TargetPath = $exe.FullName
    $Shortcut.WorkingDirectory = $exe.DirectoryName
    $Shortcut.Save()
    Write-Host "Shortcut created on Desktop: Anshu.lnk" -ForegroundColor Green
}

Write-Host "`nDone." -ForegroundColor Cyan
