REM Batch File created by Faizan-ur-Rehman Last Updated 29-06-2017
::This loads assets from input path and load them in unity in unique directory and create asset bundle from them and make zips of them as well

set unityProjectPath=C:\Users\Faizan\Documents\AssetBundleExporter

set inputPath=C:\Users\Faizan\Desktop\AssetModelZip

set inputAssetModelName=DoubleHungWindow_Unity

set errerCode=0

IF NOT EXIST "%unityProjectPath%\Assets\" GOTO DIRECTORYNOTEXIST

::remove directory resource with all its sub directories and without confirmation

rename "%unityProjectPath%\Assets\Resources" resources_old

rmdir "%unityProjectPath%\Assets\parrentResources" /s /q

::create Resources directory in the new unique directory using build number environment variable

mkdir "%unityProjectPath%\Assets\parrentResources\%BUILD_NUMBER%\Resources"

::unzip all input zip files to new created resource folder

IF NOT EXIST "C:\Program Files\7-Zip" GOTO 7ZIPNOTINSTALLED

cd "C:\Program Files\7-Zip"

IF NOT EXIST "%inputPath%\" GOTO INPUTLOCATIONNOTFOUND

REM for %%i in ("%inputPath%\*.zip") do 7z x %%i -o%unityProjectPath%\Assets\parrentResources\%BUILD_NUMBER%\Resources

REM Now are extracting one zip instead of all so we are changing above one to

7z x %inputPath%\%inputAssetModelName%.zip -o%unityProjectPath%\Assets\parrentResources\%BUILD_NUMBER%\Resources


IF NOT EXIST "C:\Program Files\Unity\Editor\" GOTO UNITYNOTFOUND

cd "C:\Program Files\Unity\Editor"

::Now run unity in batch mode, unity project automatically detects new unique directory and create materials from .mtl files assign bundle names and export them(see the unity editor script)

start /wait Unity.exe -quit -projectPath %unityProjectPath% -executeMethod RBAssetBundleExporter.BuildAllAssetBundles %inputAssetModelName%

set scriptReturnCode=%ERRORLEVEL%

if %scriptReturnCode%==1 GOTO UNITY_PROJECT_PATH_NOT_FOUND
if %scriptReturnCode%==2 GOTO UNITY_PARRENT_RESOURCE
if %scriptReturnCode%==3 GOTO UNITY_RESOURCE_FOLDER
if %scriptReturnCode%==4 GOTO UNITY_ARGUMENT_NOT_FOUND
if %scriptReturnCode%==5 GOTO UNITY_FOLDER_NOT_FOUND
if NOT %scriptReturnCode%==0 GOTO UNITY_NOT_RUN

IF NOT EXIST "%unityProjectPath%\newAssetBundles\%inputAssetModelName%" GOTO ASSETBUNDLENOTCREATED

mkdir %unityProjectPath%\newAssetBundles\zips\%inputAssetModelName%

cd %unityProjectPath%\newAssetBundles

for %%j in (%inputAssetModelName%.*) do copy %%j zips\%inputAssetModelName%

cd "%unityProjectPath%\newAssetBundles\zips"

for /d %%i in (*.*) do 7z a -tzip %%i %%i

GOTO END

:DIRECTORYNOTEXIST

echo "unity project directory does not exists. Please create unity project named test containing editer script myscript with function BuildAllAssetBundles which exports asset bundles folder wise from resources"
set errorCode=6
GOTO END

:7ZIPNOTINSTALLED

echo "7-zip is not installed. Please make sure that it is installed and in C:\Programm Files"
set errorCode=7
GOTO END

:INPUTLOCATIONNOTFOUND

echo "Input location not found. place input zips in C:\users\faizan\desktop\input\"
set errorCode=8
GOTO END

:UNITYNOTFOUND

echo "Unity is found in the system. please make sure it is installed and in the c:\program files"
set errorCode=9
GOTO END

:OUTPUTFOLDERNOTEXISTS

echo "output folder not exists it should be in your unity project name newAssetBundles"
set errorCode=10
GOTO END

:ASSETBUNDLENOTCREATED

echo "asset bundle not created in output folder after unity script"
set errorCode=11
GOTO END

:UNITY_PROJECT_PATH_NOT_FOUND

echo "error returned from unity with code "%scriptReturnCode%" that predefined path for resources in unity project is not found"
set errorCode=%scriptReturnCode%
GOTO END

:UNITY_PARRENT_RESOURCE

echo "error returned from unity with code "%scriptReturnCode%" that parent resource does not contain single folder"
set errorCode=%scriptReturnCode%
GOTO END

:UNITY_RESOURCE_FOLDER

echo "error returned from unity with code "%scriptReturnCode%" that resource folder in predefined path is not found"
set errorCode=%scriptReturnCode%
GOTO END

:UNITY_ARGUMENT_NOT_FOUND

echo "error returned from unity with code "%scriptReturnCode%" that asset folder name as an argument was not send"
set errorCode=%scriptReturnCode%
GOTO END

:UNITY_FOLDER_NOT_FOUND

echo "error returned from unity with code "%scriptReturnCode%" that folder name sent as an argument does not exist"
set errorCode=%scriptReturnCode%
GOTO END

:UNITY_NOT_RUN

echo "unity could not be started"
set errorCode=12
GOTO END


:END

