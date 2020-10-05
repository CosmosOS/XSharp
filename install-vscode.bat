@echo off

setlocal ENABLEEXTENSIONS

cd /D "%~dp0"

if not exist "%ProgramFiles(x86)%" (
  set "ProgramFiles(x86)=%ProgramFiles%"
)

if /I "%1"=="/uninstall" goto:uninstall

:install
echo Building X#
dotnet msbuild XSharp.sln -target:"Restore;Build;Pack" -maxcpucount -verbosity:normal

echo Installing Nuget Feed
dotnet nuget remove source --name "X# Local Package Feed"
dotnet nuget add source "./artifacts/Debug/nupkg" --name "X# Local Package Feed"

echo Installing VS Code Extension
REM code uninstall
REM code install

goto:eof

:uninstall
echo Removing Nuget Feed
dotnet nuget remove source --name "X# Local Package Feed"

echo Removing VS Code Extension
REM code uninstall
