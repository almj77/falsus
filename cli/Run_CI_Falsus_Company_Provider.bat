dotnet restore ..\src\Providers\Company\Falsus.Providers.Company\Falsus.Providers.Company.csproj
dotnet build ..\src\Providers\Company\Falsus.Providers.Company\Falsus.Providers.Company.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Company\Falsus.Providers.Company.UnitTests\Falsus.Providers.Company.UnitTests.csproj --no-restore --verbosity normal

pause