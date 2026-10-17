@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\Build-Packages.ps1" %*
exit /b %errorlevel%
