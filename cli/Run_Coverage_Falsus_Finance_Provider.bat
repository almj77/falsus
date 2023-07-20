dotnet test "..\src\Providers\Finance\Falsus.Providers.Finance.UnitTests\Falsus.Providers.Finance.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Finance\TestResults"
reportgenerator "-reports:..\src\Providers\Finance\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Finance\TestResults\coveragereport" -reporttypes:Html
pause