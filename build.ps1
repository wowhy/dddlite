dotnet restore

dotnet build .\src\DDDLite -c Release
dotnet build .\src\DDDLite.Repositories.EntityFramework -c Release
dotnet build .\src\DDDLite.WebApi -c Release

dotnet pack .\src\DDDLite -c Release -o ..\..\artifacts
dotnet pack .\src\DDDLite.Repositories.EntityFramework -c Release -o ..\..\artifacts
dotnet pack .\src\DDDLite.WebApi -c Release -o ..\..\artifacts
