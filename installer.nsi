!include "MUI2.nsh"

Name "演讲倒计时器"
OutFile "SpeechTimer_Setup_1.0.0.exe"
InstallDir "$LOCALAPPDATA\SpeechTimer"
InstallDirRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "InstallDir"
RequestExecutionLevel user

Var StartMenuFolder

!define MUI_ICON "Assets\main.ico"
!define MUI_UNICON "Assets\main.ico"
!define MUI_ABORTWARNING
!define MUI_ABORTWARNING_TEXT "确定要退出 演讲倒计时器 安装程序吗？"
!define MUI_FINISHPAGE_RUN "$INSTDIR\SpeechTimer.exe"
!define MUI_FINISHPAGE_RUN_TEXT "运行 演讲倒计时器"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY

!define MUI_STARTMENUPAGE_DEFAULTFOLDER "演讲倒计时器"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT HKCU
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "StartMenuFolder"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder

!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "SimpChinese"

Section "演讲倒计时器" SecMain
  SetOutPath "$INSTDIR"
  SetOverwrite on

  File /r "bin\x64\Release\net9.0-windows\win-x64\publish\*.*"

  WriteUninstaller "$INSTDIR\uninst.exe"

  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "InstallDir" "$INSTDIR"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "Version" "1.0.0"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "DisplayName" "演讲倒计时器"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "DisplayIcon" "$INSTDIR\SpeechTimer.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "DisplayVersion" "1.0.0"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "Publisher" "SpeechTimer"
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer" "EstimatedSize" "$0"

  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\演讲倒计时器.lnk" "$INSTDIR\SpeechTimer.exe"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\卸载 演讲倒计时器.lnk" "$INSTDIR\uninst.exe"
  !insertmacro MUI_STARTMENU_WRITE_END

  CreateShortCut "$DESKTOP\演讲倒计时器.lnk" "$INSTDIR\SpeechTimer.exe"
SectionEnd

Section "Uninstall"
  Delete "$INSTDIR\uninst.exe"
  Delete "$DESKTOP\演讲倒计时器.lnk"

  RMDir /r "$INSTDIR"

  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  Delete "$SMPROGRAMS\$StartMenuFolder\演讲倒计时器.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\卸载 演讲倒计时器.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"

  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\SpeechTimer"
SectionEnd
