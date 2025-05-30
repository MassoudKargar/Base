@echo off
setlocal enabledelayedexpansion

REM پاک کردن پوشه nupkgs قبلی اگر وجود داشت
if exist "nupkgs" (
    echo Removing existing nupkgs folder...
    rmdir /s /q nupkgs
)

REM ساخت پوشه nupkgs جدید
mkdir nupkgs

REM شمارنده پروژه‌های پیدا شده
set foundCount=0

echo Searching all subfolders for .csproj files...

REM جستجو در تمام فولدرها و زیرشاخه‌ها برای فایل‌های csproj
for /r %%P in (*.csproj) do (
    set "projName=%%~nP"
    set "projNameLower=!projName:~0!"
    REM چک می‌کنیم اگر اسم پروژه شامل Sample بود رد کنیم
    echo !projName! | findstr /i "Sample" >nul
    if errorlevel 1 (
        echo Packing project: %%P
        dotnet pack "%%P" -o nupkgs
        set /a foundCount+=1
    ) else (
        echo Skipping sample project: %%P
    )
)

if %foundCount%==0 (
    echo No eligible .csproj files found to pack.
) else (
    echo Packed %foundCount% projects successfully.
)

echo All done!
pause
