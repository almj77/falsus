dotnet restore ..\src\Providers\Location\Falsus.Providers.Location\Falsus.Providers.Location.csproj
dotnet build ..\src\Providers\Location\Falsus.Providers.Location\Falsus.Providers.Location.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Location\Falsus.Providers.Location.UnitTests\Falsus.Providers.Location.UnitTests.csproj --no-restore --verbosity normal

pause