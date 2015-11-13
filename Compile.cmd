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
msbuild /m ECalc.sln /p:Configuration=Release

rem ---------------------------------------------------------------------------
rem cleanup
rem ---------------------------------------------------------------------------
cd bin\Release
del *.pdb
del *.vshost.exe.config
del *.vshost.exe
del MahApps.Metro.xml
cd ..

pause