@echo off
del Pure3DDataViewer.zip
rmdir /s /q "Publish"
mkdir "Publish"
mkdir "Publish\Plugins"

dotnet publish Pure3DDataViewer\Pure3DDataViewer\Pure3DDataViewer.csproj -c release -r win-x64 -p:PublishSingleFile=true --self-contained false -o Pure3DDataViewer\Pure3DDataViewer\bin\publish
copy Pure3DDataViewer\Pure3DDataViewer\bin\publish\Pure3DDataViewer.exe Publish\Pure3DDataViewer.exe
copy Pure3DDataViewer\Pure3DDataViewer\bin\publish\Pure3DDataViewerPluginAPI.dll Publish\Pure3DDataViewerPluginAPI.dll

dotnet publish Pure3DDataViewer\Pure3DDataViewerPluginAPIMemoryTunnel\Pure3DDataViewerPluginAPIMemoryTunnel.csproj -c release -r win-x86 -p:PublishSingleFile=true --self-contained false -o Pure3DDataViewer\Pure3DDataViewerPluginAPIMemoryTunnel\bin\publish
copy Pure3DDataViewer\Pure3DDataViewerPluginAPIMemoryTunnel\bin\publish\Pure3DDataViewerPluginAPIMemoryTunnel.exe Publish\Pure3DDataViewerPluginAPIMemoryTunnel.exe

dotnet publish Pure3DDataViewer\ImportExportImages\ImportExportImages.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\ImportExportImages\bin\publish
copy Pure3DDataViewer\ImportExportImages\bin\publish\ImportExportImages.dll Publish\Plugins\ImportExportImages.dll

dotnet publish Pure3DDataViewer\Sort\Sort.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\Sort\bin\publish
copy Pure3DDataViewer\Sort\bin\publish\Sort.dll Publish\Plugins\Sort.dll

dotnet publish Pure3DDataViewer\Deduplicate\Deduplicate.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\Deduplicate\bin\publish
copy Pure3DDataViewer\Deduplicate\bin\publish\Deduplicate.dll Publish\Plugins\Deduplicate.dll

"C:\Program Files\7-Zip\7z.exe" a -tzip Pure3DDataViewer.zip .\Publish\*

PAUSE