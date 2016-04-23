@echo off
title ECalc Build

rem ---------------------------------------------------------------------------
rem Prepare
rem ---------------------------------------------------------------------------
if exist bin\release echo Y | rmdir /s bin\release
if exist bin\release echo I | rmdir /s bin\release

rem ---------------------------------------------------------------------------
rem setup
rem ---------------------------------------------------------------------------
if %processor_architecture%==AMD64 goto x64
goto x86

:x86
call "%PROGRAMFILES%\Microsoft Visual Studio 14.0\VC\bin\vcvars32.bat"
echo "32 bit system"
goto compile

:x64
echo "64 bit system"
call "%PROGRAMFILES(x86)%\Microsoft Visual Studio 14.0\VC\bin\vcvars32.bat"
goto compile

rem ---------------------------------------------------------------------------
rem compile
rem ---------------------------------------------------------------------------
:compile
echo Running MSBuild...
msbuild /m ECalc.sln /p:Configuration=Release /l:FileLogger,Microsoft.Build.Engine;logfile=compile.log;append=false
if %errorlevel%==1 echo Build Failed. Check compile.log
if %errorlevel%==0 echo Build Succesfull.

rem ---------------------------------------------------------------------------
rem cleanup
rem ---------------------------------------------------------------------------
echo Cleaning ...
cd bin\Release
del *.pdb
del *.vshost.exe.config
del *.vshost.exe
del MahApps.Metro.xml
cd ..

pause