dotnet restore ..\src\Providers\Text\Falsus.Providers.Text\Falsus.Providers.Text.csproj
dotnet build ..\src\Providers\Text\Falsus.Providers.Text\Falsus.Providers.Text.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\Text\Falsus.Providers.Text.UnitTests\Falsus.Providers.Text.UnitTests.csproj --no-restore --verbosity normal

pause