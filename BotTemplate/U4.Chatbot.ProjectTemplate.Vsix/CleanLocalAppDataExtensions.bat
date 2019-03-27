REM Set as pre build step of this project to remove cached extension

set VS_PATH=Microsoft\VisualStudio

cd %LOCALAPPDATA%\%VS_PATH%

for /f %%G in ('dir /b "15.*Exp"') do ( 
	cd %%G\Extensions
	rmdir /S /Q Unit4
)

