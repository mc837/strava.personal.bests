dotnet tool update dotnet-reportgenerator-globaltool --tool-path Tools --version 4.6.1 --configfile nuget.config -v n

dotnet test --logger "trx;LogFileName=TestResults.trx" --logger "xunit;LogFileName=TestResults.xml" --results-directory ./BuildReports/UnitTests /p:CollectCoverage=true /p:CoverletOutput=BuildReports\Coverage\ /p:CoverletOutputFormat=cobertura /p:Exclude="[xunit.*]*


tools\reportgenerator.exe "-reports:BuildReports\Coverage\coverage.cobertura.xml" "-targetdir:BuildReports\Coverage" -reporttypes:HTML;HTMLSummary

start BuildReports\Coverage\index.htm

