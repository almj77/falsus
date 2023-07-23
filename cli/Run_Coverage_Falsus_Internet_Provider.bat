rmdir "..\src\Providers\Internet\TestResults" /s /q
dotnet test "..\src\Providers\Internet\Falsus.Providers.Internet.UnitTests\Falsus.Providers.Internet.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Internet\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\Internet\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Internet\TestResults\coveragereport" -reporttypes:Html
pause