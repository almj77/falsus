dotnet restore ..\src\Providers\Finance\Falsus.Providers.Finance\Falsus.Providers.Finance.csproj
dotnet build ..\src\Providers\Finance\Falsus.Providers.Finance\Falsus.Providers.Finance.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Finance\Falsus.Providers.Finance.UnitTests\Falsus.Providers.Finance.UnitTests.csproj --no-restore --verbosity normal

pause