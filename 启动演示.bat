@echo off
cd /d "%~dp0"
set EXE=YoloFool\bin\Release\net8.0-windows\YoloFool.exe
if exist "%EXE%" (
    start "" "%EXE%"
) else (
    dotnet run --project YoloFool
)
