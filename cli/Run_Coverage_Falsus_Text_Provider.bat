rmdir "..\src\Providers\Text\TestResults" /s /q
dotnet test "..\src\Providers\Text\Falsus.Providers.Text.UnitTests\Falsus.Providers.Text.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Text\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\Text\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Text\TestResults\coveragereport" -reporttypes:Html
pause