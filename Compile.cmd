@echo off
title ECalc Build

rem ---------------------------------------------------------------------------
rem Prepare
rem ---------------------------------------------------------------------------
if exist bin\release echo Y | rmdir /s bin\release
if exist bin\release echo I | rmdir /s bin\release
goto compile

rem ---------------------------------------------------------------------------
rem compile
rem ---------------------------------------------------------------------------
:compile
echo Compiling ...
set compiler="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe"
%compiler% ECalc.sln /Build Release 
goto clean

rem ---------------------------------------------------------------------------
rem cleanup
rem ---------------------------------------------------------------------------
:clean
echo Cleaning ...
cd bin\Release
del *.pdb
del MahApps.Metro.xml
del IronPython.*.xml
del IronPython.xml
del Microsoft.Scripting.*.xml
del Microsoft.Scripting.xml
del Microsoft.Dynamic.xml
del ICSharpCode.AvalonEdit.xml
cd ..

pause