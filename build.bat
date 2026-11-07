@echo off
setlocal

rem Prefer user-local .NET 9 SDK, fall back to dotnet on PATH
set "DOTNET=dotnet"
if exist "%LOCALAPPDATA%\Microsoft\dotnet9\dotnet.exe" set "DOTNET=%LOCALAPPDATA%\Microsoft\dotnet9\dotnet.exe"

echo [1/2] Publishing...
"%DOTNET%" publish -c Release -r win-x64 --self-contained true -p:Platform=x64
if errorlevel 1 goto :error

echo [2/2] Building installer...
"%ProgramFiles(x86)%\NSIS\makensis.exe" installer.nsi
if errorlevel 1 goto :error

echo.
echo Done: SpeechTimer_Setup_1.0.0.exe
goto :eof

:error
echo.
echo Build failed with exit code %errorlevel%
exit /b 1
