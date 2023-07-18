dotnet restore ..\src\Providers\Internet\Falsus.Providers.Internet\Falsus.Providers.Internet.csproj
dotnet build ..\src\Providers\Internet\Falsus.Providers.Internet\Falsus.Providers.Internet.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Internet\Falsus.Providers.Internet.UnitTests\Falsus.Providers.Internet.UnitTests.csproj --no-restore --verbosity normal

pause