@echo off
echo CreateBuildLog...

set PROJECT_DIR=%1
set VERSION=%2

FOR /f "tokens=1,2,3,4 delims=." %%a IN ("%VERSION%") do (
	set MAJOR=%%a
	set MINOR=%%b
	set BUILD=%%c
	set REVISION=%%d
)

set BUILDLOG_FILE=%MAJOR%.%MINOR%.log
set BUILDLOG_FOLDER=%PROJECT_DIR%\AssemblyVersion\BuildLogs

if not exist %BUILDLOG_FOLDER% mkdir %BUILDLOG_FOLDER%

echo %DATE% %TIME% >> %BUILDLOG_FOLDER%\%BUILDLOG_FILE%"
