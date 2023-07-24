rmdir "..\src\Providers\DataTypes\TestResults" /s /q
dotnet test "..\src\Providers\DataTypes\Falsus.Providers.DataTypes.UnitTests\Falsus.Providers.DataTypes.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\DataTypes\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\DataTypes\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\DataTypes\TestResults\coveragereport" -reporttypes:Html
pause