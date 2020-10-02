; Do NOT change this next line in Dev Kit
#ifndef ChangeSetVersion
#define ChangeSetVersion "0.1.0-localbuild"
#endif

#ifndef BuildConfiguration
; Currently we dont use "UserKit" but this allows us to test/compile from Inno
; IDE so that we don't get an undefined error.
; We default to devkit so we dont have to wait on compression.
#define BuildConfiguration "DevKit"
;#define BuildConfiguration "UserKit"
#endif

#if BuildConfiguration == "DevKit"
	; devkit releases are not compressed
	#pragma warning "Building Dev Kit release"
#else
	; userkit releases get compressed, and get languages included
	#pragma message "Building User Kit release"
	#define Compress true
	#define IncludeUILanguages true
#endif

[Setup]
AppId=XSharpUserKit
AppName=X# User Kit
AppVerName=X# User Kit v{#ChangeSetVersion}
AppCopyright=Copyright (c) 2007-2020 The Cosmos Project
AppPublisher=Cosmos Project
AppPublisherURL=http://www.goCosmos.org/
AppSupportURL=http://www.goCosmos.org/
AppUpdatesURL=http://www.goCosmos.org/
AppVersion={#ChangeSetVersion}
SetupMutex=XSharpSetupMutexName,Global\XSharpetupMutexName
DefaultDirName={userappdata}\XSharp User Kit
DefaultGroupName=X# User Kit
OutputDir=.\Setup\Output
OutputBaseFilename=XSharpUserKit-{#ChangeSetVersion}
#ifdef Compress
Compression=lzma2/ultra64
InternalCompressLevel=ultra64
SolidCompression=true
#else
Compression=none
InternalCompressLevel=none
#endif
SourceDir=..
;Left Image should be 164x314
;WizardImageFile=xsharp.bmp
;Small Image should be 55x55
;WizardSmallImageFile=xsharp.bmp
AllowCancelDuringInstall=false
UninstallLogMode=overwrite
ChangesAssociations=yes
DisableWelcomePage=True
DisableReadyPage=True
DisableReadyMemo=True
FlatComponentsList=False
AlwaysShowComponentsList=False
ShowComponentSizes=False
LicenseFile=LICENSE.txt
DisableDirPage=no

[Messages]
SelectDirDesc=If the user installing the X# User Kit is not the admin. Please choose the corresponding AppData/Roaming directory.

[Dirs]
Name: {app}; Flags: uninsalwaysuninstall

[InstallDelete]
Type: filesandordirs; Name: "{app}"

[Files]
; Packages
Source: ".\artifacts\Debug\nupkg\*.nupkg"; DestDir: "{app}\packages\"; Flags: ignoreversion uninsremovereadonly
; Icon
Source: ".\XSharp.ico"; DestDir: "{app}"; Flags: ignoreversion uninsremovereadonly
; VS Code
; Source: ".\artifacts\Debug\vscode\XSharp.zip"; DestDir: "{app}\Build\Tools"; Flags: ignoreversion uninsremovereadonly

[Registry]
; Regiter .xs Extension
Root: HKCR; Subkey: ".xs"; ValueType: string; ValueName: ""; ValueData: "XSharp"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "XSharp"; ValueType: string; ValueName: ""; ValueData: "X# source file"; Flags: uninsdeletekey
Root: HKCR; Subkey: "XSharp\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\XSharp.ico,0"
; Root: HKCR; Subkey: "XSharp\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Build\XSharp\XSC.exe"" ""%1"""

; User Kit Folder
Root: HKLM; SubKey: Software\XSharp; ValueType: string; ValueName: "UserKit"; ValueData: {app}; Flags: uninsdeletekey
; Dev Kit Folder - Set by builder only, but we delete it here. See comments in builder.
; HKCU because Builder doesn't run as admin
; Note HKCU is not part of registry redirection
Root: HKCU; SubKey: Software\XSharp; ValueType: none; ValueName: "DevKit"; Flags: deletekey

[ThirdParty]
UseRelativePaths=True

[Run]
Filename: "dotnet.exe"; Parameters: "nuget remove source --name ""X# Local Package Feed"""; WorkingDir: "{app}"; Description: "Uninstall X# Packages"; StatusMsg: "Uninstalling X# Packages"
Filename: "dotnet.exe"; Parameters: "nuget add source ""{app}\packages\\"" --name ""X# Local Package Feed"""; WorkingDir: "{app}"; Description: "Install X# Packages"; StatusMsg: "Installing X# Packages"

; Filename: "code.exe"; Parameters: "uninstall"; Description: "Remove X# VS Code Extension"; StatusMsg: "Removing X# VS Code Extension"
; Filename: "code.exe"; Parameters: "install"; Description: "Install X# VS Code Extension"; StatusMsg: "Installing X# VS Code Extension"

[UninstallRun]
Filename: "dotnet.exe"; Parameters: "nuget remove source --name ""X# Local Package Feed"""; WorkingDir: "{app}"; StatusMsg: "Uninstalling X# Packages"
; Filename: "code.exe"; Parameters: "uninstall"; StatusMsg: "Removing X# VS Code Extension"

[Code]

function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;

/////////////////////////////////////////////////////////////////////
// Uninstall previously installed application.
/////////////////////////////////////////////////////////////////////
function UnInstallOldVersion(): Integer;
var
  sUnInstallString: String;
  iResultCode: Integer;
begin
// Return Values:
// 1 - uninstall string is empty
// 2 - error executing the UnInstallString
// 3 - successfully executed the UnInstallString

  // default return value
  Result := 0;

  // get the uninstall string of the old app
  sUnInstallString := GetUninstallString();
  if sUnInstallString <> '' then begin
    sUnInstallString := RemoveQuotes(sUnInstallString);
    if Exec(sUnInstallString, '/SILENT /NORESTART /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode) then
      Result := 3
    else
      Result := 2;
  end else
    Result := 1;
end;

[Languages]
Name: en; MessagesFile: compiler:Default.isl; InfoBeforeFile: .\setup\Readme.txt
#ifdef IncludeUILanguages
Name: eu; MessagesFile: .\setup\Languages\Basque-1-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: ca; MessagesFile: .\setup\Languages\Catalan-4-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: cs; MessagesFile: .\setup\Languages\Czech-5-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: da; MessagesFile: .\setup\Languages\Danish-4-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: nl; MessagesFile: .\setup\Languages\Dutch-8-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: fi; MessagesFile: .\setup\Languages\Finnish-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: fr; MessagesFile: .\setup\Languages\French-15-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: de; MessagesFile: .\setup\Languages\German-2-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: hu; MessagesFile: .\setup\Languages\Hungarian-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: it; MessagesFile: .\setup\Languages\Italian-14-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: no; MessagesFile: .\setup\Languages\Norwegian-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: pl; MessagesFile: .\setup\Languages\Polish-8-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: pt; MessagesFile: .\setup\Languages\PortugueseStd-1-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: ru; MessagesFile: .\setup\Languages\Russian-19-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: sk; MessagesFile: .\setup\Languages\Slovak-6-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: sl; MessagesFile: .\setup\Languages\Slovenian-3-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
;Unofficial:
Name: bg; MessagesFile: .\setup\Languages\InOfficial\Bulgarian-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: el; MessagesFile: .\setup\Languages\InOfficial\Greek-4-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: is; MessagesFile: .\setup\Languages\InOfficial\Icelandic-1-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: id; MessagesFile: .\setup\Languages\InOfficial\Indonesian-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: ja; MessagesFile: .\setup\Languages\InOfficial\Japanese-5-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: ko; MessagesFile: .\setup\Languages\InOfficial\Korean-5-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: ms; MessagesFile: .\setup\Languages\InOfficial\Malaysian-2-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: es; MessagesFile: .\setup\Languages\InOfficial\SpanishStd-2-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: sv; MessagesFile: .\setup\Languages\InOfficial\Swedish-8-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: tr; MessagesFile: .\setup\Languages\InOfficial\Turkish-3-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: uk; MessagesFile: .\setup\Languages\InOfficial\Ukrainian-5-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: cn; MessagesFile: .\setup\Languages\InOfficial\ChineseSimp-11-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
Name: tw; MessagesFile: .\setup\Languages\InOfficial\ChineseTrad-2-5.1.0.isl; InfoBeforeFile: .\setup\Readme.txt
#endif

[Messages]
en.BeveledLabel=English
#ifdef IncludeUILanguages
eu.BeveledLabel=Basque
ca.BeveledLabel=Catalan
cs.BeveledLabel=Czech
da.BeveledLabel=Danish
nl.BeveledLabel=Dutch
fi.BeveledLabel=Finnish
fr.BeveledLabel=French
de.BeveledLabel=German
hu.BeveledLabel=Hungarian
it.BeveledLabel=Italian
no.BeveledLabel=Norwegian
pl.BeveledLabel=Polish
pt.BeveledLabel=Portuguese
ru.BeveledLabel=Russian
sk.BeveledLabel=Slovak
sl.BeveledLabel=Slovenian
;Unofficial:
bg.BeveledLabel=Bulgarian
el.BeveledLabel=Greek
is.BeveledLabel=Icelandic
id.BeveledLabel=Indonesian
ja.BeveledLabel=Japanese
ko.BeveledLabel=Korean
ms.BeveledLabel=Malaysian
es.BeveledLabel=Spanish
sv.BeveledLabel=Swedish
tr.BeveledLabel=Turkish
uk.BeveledLabel=Ukrainian
cn.BeveledLabel=Chinese Simplified
tw.BeveledLabel=Chinese Traditional
#endif
