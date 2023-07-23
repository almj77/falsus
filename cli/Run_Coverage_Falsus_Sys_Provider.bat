rmdir "..\src\Providers\Sys\TestResults" /s /q
dotnet test "..\src\Providers\Sys\Falsus.Providers.Sys.UnitTests\Falsus.Providers.Sys.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Sys\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\Sys\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Sys\TestResults\coveragereport" -reporttypes:Html
pause