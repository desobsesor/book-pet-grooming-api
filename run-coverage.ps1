# Script to run tests and generate coverage reports

# Install ReportGenerator globally if not installed
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing ReportGenerator globally..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
}

# Create directory for reports if it does not exist
$reportsDir = "./coverage-reports"
if (-not (Test-Path $reportsDir)) {
    New-Item -ItemType Directory -Path $reportsDir | Out-Null
}

# Clean previous reports
Remove-Item "$reportsDir/*" -Recurse -Force -ErrorAction SilentlyContinue

# Run unit tests with coverage
Write-Host "Running unit tests with coverage..."
dotnet test ./tests/BookPetGroomingAPI.UnitTests/BookPetGroomingAPI.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory:"$reportsDir/unit" --settings:./coverage.runsettings

# Run integration tests with coverage
Write-Host "Running integration tests with coverage..."
dotnet test ./tests/BookPetGroomingAPI.IntegrationTests/BookPetGroomingAPI.IntegrationTests.csproj --collect:"XPlat Code Coverage" --results-directory:"$reportsDir/integration" --settings:./coverage.runsettings

# Find generated coverage files
$coverageFiles = Get-ChildItem -Path "$reportsDir" -Recurse -Filter "coverage.cobertura.xml" | Select-Object -ExpandProperty FullName

# Generate combined HTML report
Write-Host "Generating HTML coverage report..."
reportgenerator "-reports:$($coverageFiles -join ';')" "-targetdir:$reportsDir/html" -reporttypes:"Html;HtmlSummary;Cobertura;JsonSummary;TextSummary" -title:"BookPetGroomingAPI Coverage Report"

# Display coverage summary
Get-Content "$reportsDir/html/Summary.txt"

# Open the report in the default browser
Write-Host "Opening report in the browser..."
Start-Process "$reportsDir/html/index.html"

Write-Host "Process completed. The coverage report is available at: $reportsDir/html/index.html"