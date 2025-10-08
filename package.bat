@echo off
setlocal

REM --- Check if EtheriumModPackager is in PATH ---
where EtheriumModPackager.exe >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo ERROR: EtheriumModPackager.exe was not found in your PATH.
    echo Please make sure the folder containing EtheriumModPackager.exe is added to your PATH.
    exit /b 1
)

REM --- Set the mod directory path ---
set "MOD_DIR=%CD%\bin\Debug\net35"

REM --- Check if the directory exists ---
if not exist "%MOD_DIR%" (
    echo ERROR: Mod directory "%MOD_DIR%" does not exist.
    exit /b 1
)

echo Packaging mod from: "%MOD_DIR%"

REM --- Run EtheriumModPackager on the directory ---
EtheriumModPackager.exe "%MOD_DIR%"

if %ERRORLEVEL% equ 0 (
    pause
) else (
    echo ERROR: Failed to package mod.
    exit /b 1
)

endlocal
