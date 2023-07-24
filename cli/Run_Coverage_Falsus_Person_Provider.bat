rmdir "..\src\Providers\Person\TestResults" /s /q
dotnet test "..\src\Providers\Person\Falsus.Providers.Person.UnitTests\Falsus.Providers.Person.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Person\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\Person\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Person\TestResults\coveragereport" -reporttypes:Html
pause