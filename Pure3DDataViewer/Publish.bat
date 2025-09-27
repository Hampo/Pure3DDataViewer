@echo off
del Pure3DDataViewer.zip
rmdir /s /q "Publish"
mkdir "Publish"
mkdir "Publish\Plugins"

dotnet publish Pure3DDataViewer\Pure3DDataViewer.csproj -c release -r win-x64 -p:PublishSingleFile=true --self-contained false -o Pure3DDataViewer\bin\publish
copy Pure3DDataViewer\bin\publish\Pure3DDataViewer.exe Publish\Pure3DDataViewer.exe
copy Pure3DDataViewer\bin\publish\Pure3DDataViewerPluginAPI.dll Publish\Pure3DDataViewerPluginAPI.dll

dotnet publish ImportExportImages\ImportExportImages.csproj -c release -r win-x64 --self-contained false -o ImportExportImages\bin\publish
copy ImportExportImages\bin\publish\ImportExportImages.dll Publish\Plugins\ImportExportImages.dll

dotnet publish Sort\Sort.csproj -c release -r win-x64 --self-contained false -o Sort\bin\publish
copy Sort\bin\publish\Sort.dll Publish\Plugins\Sort.dll

dotnet publish Deduplicate\Deduplicate.csproj -c release -r win-x64 --self-contained false -o Deduplicate\bin\publish
copy Deduplicate\bin\publish\Deduplicate.dll Publish\Plugins\Deduplicate.dll

"C:\Program Files\7-Zip\7z.exe" a -tzip Pure3DDataViewer.zip .\Publish\*

PAUSE