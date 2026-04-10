@echo off
del Pure3DDataViewer.zip
rmdir /s /q "Publish"
mkdir "Publish"
mkdir "Publish\Plugins"

dotnet publish Pure3DDataViewer\Pure3DDataViewer\Pure3DDataViewer.csproj -c release -r win-x64 -p:PublishSingleFile=true --self-contained false -o Pure3DDataViewer\Pure3DDataViewer\bin\publish
copy Pure3DDataViewer\Pure3DDataViewer\bin\publish\Pure3DDataViewer.exe Publish\Pure3DDataViewer.exe
copy Pure3DDataViewer\Pure3DDataViewer\bin\publish\Pure3DDataViewerPluginAPI.dll Publish\Pure3DDataViewerPluginAPI.dll

dotnet publish Pure3DDataViewer\ImportExportImages\ImportExportImages.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\ImportExportImages\bin\publish
copy Pure3DDataViewer\ImportExportImages\bin\publish\ImportExportImages.dll Publish\Plugins\ImportExportImages.dll

dotnet publish Pure3DDataViewer\Sort\Sort.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\Sort\bin\publish
copy Pure3DDataViewer\Sort\bin\publish\Sort.dll Publish\Plugins\Sort.dll

dotnet publish Pure3DDataViewer\Deduplicate\Deduplicate.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\Deduplicate\bin\publish
copy Pure3DDataViewer\Deduplicate\bin\publish\Deduplicate.dll Publish\Plugins\Deduplicate.dll

dotnet publish Pure3DDataViewer\LocationFromGame\LocationFromGame.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\LocationFromGame\bin\publish
copy Pure3DDataViewer\LocationFromGame\bin\publish\LocationFromGame.dll Publish\Plugins\LocationFromGame.dll

dotnet publish Pure3DDataViewer\FrontendTextBibleEditor\FrontendTextBibleEditor.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\FrontendTextBibleEditor\bin\publish
copy Pure3DDataViewer\FrontendTextBibleEditor\bin\publish\FrontendTextBibleEditor.dll Publish\Plugins\FrontendTextBibleEditor.dll

dotnet publish Pure3DDataViewer\Validate\Validate.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\Validate\bin\publish
copy Pure3DDataViewer\Validate\bin\publish\Validate.dll Publish\Plugins\Validate.dll

dotnet publish Pure3DDataViewer\CarPhysicsObjectGenerator\CarPhysicsObjectGenerator.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\CarPhysicsObjectGenerator\bin\publish
copy Pure3DDataViewer\CarPhysicsObjectGenerator\bin\publish\CarPhysicsObjectGenerator.dll Publish\Plugins\CarPhysicsObjectGenerator.dll

dotnet publish Pure3DDataViewer\TimeOfDayTint\TimeOfDayTint.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\TimeOfDayTint\bin\publish
copy Pure3DDataViewer\TimeOfDayTint\bin\publish\TimeOfDayTint.dll Publish\Plugins\TimeOfDayTint.dll

dotnet publish Pure3DDataViewer\ConvertToLua\ConvertToLua.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\ConvertToLua\bin\publish
copy Pure3DDataViewer\ConvertToLua\bin\publish\ConvertToLua.dll Publish\Plugins\ConvertToLua.dll

dotnet publish Pure3DDataViewer\CompositeDrawableEditor\CompositeDrawableEditor.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\CompositeDrawableEditor\bin\publish
copy Pure3DDataViewer\CompositeDrawableEditor\bin\publish\CompositeDrawableEditor.dll Publish\Plugins\CompositeDrawableEditor.dll

dotnet publish Pure3DDataViewer\LocatorEditor\LocatorEditor.csproj -c release -r win-x64 --self-contained false -o Pure3DDataViewer\LocatorEditor\bin\publish
copy Pure3DDataViewer\LocatorEditor\bin\publish\LocatorEditor.dll Publish\Plugins\LocatorEditor.dll

"C:\Program Files\7-Zip\7z.exe" a -tzip Pure3DDataViewer.zip .\Publish\*

PAUSE