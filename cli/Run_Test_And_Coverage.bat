dotnet test "..\src\Falsus.sln" --collect:"XPlat Code Coverage" --results-directory "..\src\TestResults"
reportgenerator "-reports:..\src\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\TestResults\coveragereport" -reporttypes:Html
pause