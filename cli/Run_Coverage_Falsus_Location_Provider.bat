dotnet test "..\src\Providers\Location\Falsus.Providers.Location.UnitTests\Falsus.Providers.Location.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Location\TestResults"
reportgenerator "-reports:..\src\Providers\Location\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Location\TestResults\coveragereport" -reporttypes:Html
pause