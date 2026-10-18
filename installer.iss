; Inno Setup script for K-Browser. Build with build-installer.ps1.
#ifndef AppVersion
  #define AppVersion "5.2.1.0"
#endif
#ifndef PublishDir
  #define PublishDir "publish"
#endif

#define AppName "K-Browser"
#define AppExe "K-Browser.exe"

[Setup]
AppId={{B3A6F0C2-5D1E-4C57-9E0A-7C2D8F41A6B9}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Dennis Kon
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir=installer
OutputBaseFilename=K-Browser-Setup-{#AppVersion}
SetupIconFile=favicon.ico
UninstallDisplayIcon={app}\{#AppExe}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequiredOverridesAllowed=dialog
MinVersion=10.0
#ifexist "LICENSE"
LicenseFile=LICENSE
#endif

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Excludes: "*.pdb"; Flags: recursesubdirs createallsubdirs ignoreversion
; Licence notices for bundled third-party components (Fluent UI System Icons font, MIT)
Source: "ThirdPartyNotices.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExe}"
Name: "{group}\Uninstall {#AppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExe}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  WebView2ClientKey = 'SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}';

function WebView2Installed: Boolean;
var
  Version: String;
begin
  Result :=
    (RegQueryStringValue(HKLM32, WebView2ClientKey, 'pv', Version) and (Version <> '') and (Version <> '0.0.0.0')) or
    (RegQueryStringValue(HKCU, WebView2ClientKey, 'pv', Version) and (Version <> '') and (Version <> '0.0.0.0'));
end;

// True only if the file carries a valid Authenticode signature issued to Microsoft Corporation.
// The setup program is downloaded over the network and then run, so it must not be run unverified.
function IsMicrosoftSigned(const FileName: String): Boolean;
var
  ResultCode: Integer;
  Command, QuotedPath: String;
begin
  // A single quote in the path (e.g. in a user name) must be doubled inside a PowerShell string.
  QuotedPath := FileName;
  StringChangeEx(QuotedPath, '''', '''''', True);
  Command :=
    '-NoProfile -ExecutionPolicy Bypass -Command "$s = Get-AuthenticodeSignature -LiteralPath ''' +
    QuotedPath + '''; ' +
    'if ($s.Status -eq ''Valid'' -and $s.SignerCertificate.Subject -like ''*O=Microsoft Corporation*'') ' +
    '{ exit 0 } else { exit 1 }"';
  Result := Exec(ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'), Command, '', SW_HIDE,
                 ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
end;

// Installs the WebView2 runtime (required by the browser engine) when it is missing.
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  Bootstrapper: String;
  ResultCode: Integer;
begin
  Result := '';
  if WebView2Installed then
    Exit;

  try
    DownloadTemporaryFile('https://go.microsoft.com/fwlink/p/?LinkId=2124703',
      'MicrosoftEdgeWebview2Setup.exe', '', nil);
    Bootstrapper := ExpandConstant('{tmp}\MicrosoftEdgeWebview2Setup.exe');
    if not IsMicrosoftSigned(Bootstrapper) then
    begin
      Result := 'The downloaded Microsoft Edge WebView2 setup program does not have a valid Microsoft ' +
                'signature, so it was not run. Install the WebView2 Runtime from ' +
                'https://developer.microsoft.com/microsoft-edge/webview2/ and run Setup again.';
      Exit;
    end;
    if not Exec(Bootstrapper, '/silent /install', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
       or not WebView2Installed then
      Result := 'The Microsoft Edge WebView2 Runtime could not be installed. ' +
                'Install it from https://developer.microsoft.com/microsoft-edge/webview2/ and run Setup again.';
  except
    Result := 'The Microsoft Edge WebView2 Runtime is required but could not be downloaded (' +
              GetExceptionMessage + '). Check your internet connection and run Setup again.';
  end;
end;
