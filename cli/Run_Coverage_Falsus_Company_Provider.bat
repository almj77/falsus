rmdir "..\src\Providers\Company\TestResults" /s /q
dotnet test "..\src\Providers\Company\Falsus.Providers.Company.UnitTests\Falsus.Providers.Company.UnitTests.csproj" --collect:"XPlat Code Coverage" --results-directory "..\src\Providers\Company\TestResults" --settings runsettings.xml
reportgenerator "-reports:..\src\Providers\Company\TestResults\*\coverage.cobertura.xml" "-targetdir:..\src\Providers\Company\TestResults\coveragereport" -reporttypes:Html
pause