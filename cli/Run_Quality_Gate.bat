echo on
echo "Install dependencies"
dotnet restore ..\src\Falsus.sln

echo "Build the Falsus Engine"
dotnet build ..\src\Falsus.sln --configuration Release --no-restore
  
echo "Execute all unit tests for Falsus Engine"
dotnet test ..\src\Falsus.sln --no-restore --verbosity normal

pause