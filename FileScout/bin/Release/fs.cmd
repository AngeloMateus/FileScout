@echo off
call "FileScout.exe" %1
set /p LASTDIR=<%~dp0fileScoutDir
pushd  %LASTDIR%
