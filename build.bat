@echo off
:: Bat dosyasının bulunduğu dizine geç
cd /d "%~dp0"
powershell.exe -ExecutionPolicy Bypass -File "C:\Toltek\Build\BuildWithoutPrefix.ps1" -Action "build" -SolutionPath "AspNetCore.Diagnostics.HealthChecks.sln"  -PackageFolder "C:\Toltek\TLibrary\Framework"
echo.
echo Script execution finished. Press any key to exit...
pause >nul
