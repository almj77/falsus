dotnet restore ..\src\Providers\Sys\Falsus.Providers.Sys\Falsus.Providers.Sys.csproj
dotnet build ..\src\Providers\Sys\Falsus.Providers.Sys\Falsus.Providers.Sys.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Sys\Falsus.Providers.Sys.UnitTests\Falsus.Providers.Sys.UnitTests.csproj --no-restore --verbosity normal

pause