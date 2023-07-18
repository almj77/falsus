dotnet restore ..\src\Providers\Person\Falsus.Providers.Person\Falsus.Providers.Person.csproj
dotnet build ..\src\Providers\Person\Falsus.Providers.Person\Falsus.Providers.Person.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Person\Falsus.Providers.Person.UnitTests\Falsus.Providers.Person.UnitTests.csproj --no-restore --verbosity normal

pause