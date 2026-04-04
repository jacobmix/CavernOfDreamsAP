@echo off
setlocal enabledelayedexpansion
cd %~dp0

REM ============================================
REM Check admin
NET SESSION >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    ECHO ERROR: You must run this script as Administrator OR enable Developer Mode.
    PAUSE
    EXIT /B
)

REM ============================================
REM Folder picker using OpenFileDialog
SET "PScommand="POWERSHELL Add-Type -AssemblyName System.Windows.Forms; $FolderBrowse = New-Object System.Windows.Forms.OpenFileDialog -Property @{ValidateNames = $false;CheckFileExists = $false;RestoreDirectory = $true;FileName = 'Selected Folder';};$null = $FolderBrowse.ShowDialog();$FolderName = Split-Path -Path $FolderBrowse.FileName;Write-Output $FolderName""
FOR /F "usebackq tokens=*" %%Q in (`%PScommand%`) DO (
    ECHO %%Q
	SET SOURCE=%%Q
)

IF NOT DEFINED SOURCE (
    ECHO No folder selected. Exiting.
    PAUSE
    EXIT /B
)

ECHO Selected folder: "!SOURCE!"
ECHO.

REM ============================================
REM Remove existing DLLs/symlinks
DEL /Q "*.dll" >nul 2>&1

REM ============================================
REM Create symlinks for all DLLs in selected folder
FOR %%F IN ("%SOURCE%\*.dll") DO (
    ECHO Linking %%~nxF
    mklink "%%~nxF" "%%F" >nul 2>&1
    IF ERRORLEVEL 1 (
        ECHO Failed to link %%~nxF
    )
)

ECHO.
ECHO All DLLs linked successfully.
PAUSE