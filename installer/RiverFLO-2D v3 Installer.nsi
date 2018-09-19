;--------------------------------
;Include Modern UI
!include "MUI2.nsh"
!include "EnvVarUpdate.nsh"

;Defines to control which installation to create
!define NET_WEB_INSTALL
!define ARGUS_NETWORK

;Target .NET Version
!define NETVersion "3.5"

;Choose whether the installation should download .NET from the
; internet or embed it on the installer
!ifdef NET_WEB_INSTALL
	!define NETInstaller "dotnetfx35setup.exe"
!else
	!define NETInstaller "dotnetfx35.exe"
!endif

;Choose which license to use for ArgusONE
!ifdef ARGUS_NETWORK
	!define ArgusOneInstaller "Argus_ONE_Networks_Installer.exe"
!else
	!define ArgusOneInstaller "Argus_ONE_PC_Installer.exe"
!endif

;Name and file
Name "RiverFLO-2Dv3"

;Choose the installer filename depending on the install options
;selected using the defines at the beggining of the file
!ifdef NET_WEB_INSTALL
	!ifdef ARGUS_NETWORK
		OutFile "RiverFLO-2Dv3_Network_Setup (.NET Web Install).exe"
	!else
		OutFile "RiverFLO-2Dv3_Setup_PC (.NET Web Install).exe"
	!endif
!else
	!ifdef ARGUS_NETWORK
		OutFile "RiverFLO-2Dv3_Setup_Networks (.NET Standalone Install).exe"
	!else
		OutFile "RiverFLO-2Dv3_Network_Setup.exe (.NET Standalone Install).exe"
	!endif
!endif

;Default installation folder
InstallDir "$PROGRAMFILES"
  
;Get installation folder from registry if available
InstallDirRegKey HKCU "Software\FLO-2D\RiverFLO-2Dv3" ""

;Request application privileges for Windows 7
RequestExecutionLevel admin
  
ShowInstDetails show

;SetCompress off

;--------------------------------
;Variables
Var StartMenuFolder

;--------------------------------
;Interface Settings
!define MUI_ABORTWARNING
!define MUI_ICON "RiverFLO-2Dv3.ico"
;--------------------------------
;Pages

!insertmacro MUI_PAGE_WELCOME

;Start Menu Folder Page Configuration
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\FLO-2D\RiverFLO-2Dv3" 
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
 
!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES



;--------------------------------
;Languages
!insertmacro MUI_LANGUAGE "English"

Section "RiverFLO" SecDummy

	SetOutPath "$INSTDIR"
	
	IfFileExists "$WINDIR\Microsoft.NET\Framework\v${NETVersion}" endNetFramework 0
		 MessageBox MB_YESNO "You need the .NET Framework v3.5. Would you like to install it now?." /SD IDYES IDNO endNetFramework
			File /oname=$TEMP\${NETInstaller} ${NETInstaller}
			DetailPrint "Starting Microsoft .NET Framework v${NETVersion} Setup..."
			ExecWait "$TEMP\${NETInstaller}"
	endNetFramework:
	
	DetailPrint "Microsoft .NET Framework is already installed."
  
        ; redirect output to my documents
	SetOutPath "$DOCUMENTS"
	;Install River-FLO
	File /r "RiverFLO-2Dv3"
	
	SetOutPath "$SYSDIR"
	File SimpleplotDVF6.dll
	File SimpleplotImg.dll
	File SimpleplotTri.dll
	
	SetOutPath "$INSTDIR"
  
	;Create uninstaller
	WriteUninstaller "$INSTDIR\Argus Interware\ArgusPIE\RiverFLO-2Dv3\Uninstall.exe"
	
	;StartMenu shortcuts
	SetOutPath "$INSTDIR\Argus Interware\ArgusPIE\RiverFLO-2Dv3\"
	!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
		CreateDirectory "$INSTDIR\$StartMenuFolder"
		CreateShortCut "$INSTDIR\$StartMenuFolder\RiverFLO-2Dv3.lnk" "$INSTDIR\Argus Interware\ArgusPIE\RiverFLO-2Dv3\RiverFLO-2D3.exe"
		CreateShortCut "$INSTDIR\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\RiverFLO-2Dv3\Uninstall.exe"
	!insertmacro MUI_STARTMENU_WRITE_END
	
	;Desktop shortcut
	CreateShortCut "$DESKTOP\RiverFLO-2Dv3.lnk" "$INSTDIR\Argus Interware\ArgusPIE\RiverFLO-2Dv3\RiverFLO-2D3.exe"
	
	;Add shortcut to Add/Remove Programs in the Control Panel
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\RiverFLO-2Dv3" \
                 "DisplayName" "RiverFLO-2Dv3"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\RiverFLO-2Dv3" \
					 "UninstallString" "$\"$INSTDIR\Argus Interware\ArgusPIE\RiverFLO-2Dv3\Uninstall.exe$\""

SectionEnd

Section "-Argus"

	!ifdef ARGUS_NETWORK
		${EnvVarUpdate} $0 "ARGUSONE_FLOATING_LICENSE" "A" "HKLM" "1"
	!endif

	SetOutPath "$INSTDIR"
	
	;Run ArgusONE installer
	File /oname=$TEMP\${ArgusOneInstaller} ${ArgusOneInstaller}
	DetailPrint "Installing ArgusONE ... "
	ExecWait "$TEMP\${ArgusOneInstaller}"
	IfErrors error
	
	;Copy ArgusONE-RiverFLO files
	File /r "Argus Interware"
	Return
	
	error:
		MessageBox MB_OK "There was a problem installing ArgusONE. This installation will terminate."
		Quit
	
SectionEnd



;Uninstaller
Section "Uninstall"

	;Delete files
	Delete "$PROGRAMFILES\Argus Interware\ArgusPIE\RiverFLO-2Dv3\Uninstall.exe"
	RMDir /r "$PROGRAMFILES\Argus Interware\ArgusPIE\RiverFLO-2Dv3"
	RMDir /r "$DOCUMENTS\RiverFLO-2Dv3\"
	
	;Delete StartMenu shortcuts
	!insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
	Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
	Delete "$SMPROGRAMS\$StartMenuFolder\RiverFLO-2Dv3.lnk"
	Delete "$DESKTOP\RiverFLO-2Dv3.lnk"
	RMDir "$SMPROGRAMS\$StartMenuFolder"
	
	${un.EnvVarUpdate} $0 "ARGUSONE_FLOATING_LICENSE" "R" "HKLM" "1" 
	
	;Remove from Add/Remove Programs from the Control Panel
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\RiverFLO-2Dv3"
   
SectionEnd