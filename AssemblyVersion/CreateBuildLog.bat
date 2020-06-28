@echo off

REM --Get parameters
set PROJECT_DIR=%1
set VERSION_NUMBER=%2

REM --Set what to log
set LOG_LINE=%DATE% %TIME%

REM --Inform the user
set MSG=CreateBuildLog '%LOG_LINE%' for version %VERSION_NUMBER%
echo %MSG%

REM --Get version parts
FOR /f "tokens=1,2,3,4 delims=." %%a IN ("%VERSION_NUMBER%") do (
	set MAJOR=%%a
	set MINOR=%%b
	set BUILD=%%c
	set REVISION=%%d
)

REM --Define BuildLog file and folder 
set BUILDLOG_FILE=%MAJOR%.%MINOR%.log
set BUILDLOG_FOLDER=%PROJECT_DIR%\AssemblyVersion\BuildLogs

REM --Write current date and time as new line in the file
echo %LOG_LINE% >> %BUILDLOG_FOLDER%\%BUILDLOG_FILE%"
