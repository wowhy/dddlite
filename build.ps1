dotnet restore

dotnet build .\src\DDDLite -c Release
dotnet build .\src\DDDLite.Repositories.EntityFramework -c Release
dotnet build .\src\DDDLite.WebApi -c Release

dotnet pack .\src\DDDLite -c Release -o ..\..\artifacts --version-suffix=$env:APPVEYOR_BUILD_NUMBER
dotnet pack .\src\DDDLite.Repositories.EntityFramework -c Release -o ..\..\artifacts --version-suffix=$env:APPVEYOR_BUILD_NUMBER
dotnet pack .\src\DDDLite.WebApi -c Release -o ..\..\artifacts --version-suffix=$env:APPVEYOR_BUILD_NUMBER

dotnet nuget push .\artifacts\*.nupkg -k b9911133-6c64-4c93-8954-042141e984a3 --source https://www.myget.org/F/wowhy/api/v2/package
