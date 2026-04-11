@echo off
TITLE Confictura Boot Sequence
COLOR 0A

cls
echo [System] Initiating Cold Start for Confictura RunUO Server...
echo [System] Handing process control to internal C# architecture...
echo.

:: Launch the server and detach
start "" "ConficturaServer.exe"

:: Exit the batch script immediately. The console window for the server will remain open.
exit