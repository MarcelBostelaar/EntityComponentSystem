@echo off
copy /Y ".\BuildTest\settings.json" ".\Code\ComponentGeneratorStandalone\bin\Debug\netcoreapp2.0\settings.json" > nul
del /Q ".\BuildTest\Build"
mkdir ".\BuildTest\Build"
cd ".\Code\ComponentGeneratorStandalone\bin\Debug\netcoreapp2.0" > nul
@echo on
dotnet "ComponentGeneratorStandalone.dll"
@echo off
cd "..\..\..\..\.." > nul