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
set msbuildp=%windir%\Microsoft.NET\Framework\v4.0.30319\
SET PATH=%PATH%;%msbuildp%;

rem ---------------------------------------------------------------------------
rem compile
rem ---------------------------------------------------------------------------
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