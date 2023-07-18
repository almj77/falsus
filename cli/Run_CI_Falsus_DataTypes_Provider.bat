dotnet restore ..\src\Providers\DataTypes\Falsus.Providers.DataTypes\Falsus.Providers.DataTypes.csproj
dotnet build ..\src\Providers\DataTypes\Falsus.Providers.DataTypes\Falsus.Providers.DataTypes.csproj --configuration Release --no-restore
dotnet test ..\src\Providers\DataTypes\Falsus.Providers.DataTypes.UnitTests\Falsus.Providers.DataTypes.UnitTests.csproj --no-restore --verbosity normal

pause