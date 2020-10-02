@echo off

setlocal ENABLEEXTENSIONS

cd /D "%~dp0"

if not exist "%ProgramFiles(x86)%" (
  set "ProgramFiles(x86)=%ProgramFiles%"
)

set INNO_REG_KEY="HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1"
set INNO_REG_VALUE="InstallLocation"
for /f "usebackq skip=2 tokens=1-2*" %%A in (`(REG QUERY %INNO_REG_KEY% /v %INNO_REG_VALUE% /reg:32^) 2^>nul`) DO (
    set InnoSetupInstallDir=%%C
)

echo Building X#
dotnet msbuild XSharp.sln -target:"Restore;Build;Pack" -maxcpucount -verbosity:normal

echo Building Installer
set "InnoSetupCompiler=%InnoSetupInstallDir%\ISCC.exe"
if not exist "%InnoSetupCompiler%" (
    echo Inno Setup not found.
    pause
    goto:eof
)
"%InnoSetupCompiler%" ./Setup/XSharp.iss

echo Executing Installer
set InstallerExe=".\setup\output\XSharpUserKit-0.1.0-localbuild.exe"
if exist "%InstallerExe%" (
    %InstallerExe%
)
