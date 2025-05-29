# Script to generate integration tests for endpoints based on Swagger/OpenAPI

# Ensure the tools directory exists
$toolsDir = "./tools"
if (-not (Test-Path $toolsDir)) {
    New-Item -ItemType Directory -Path $toolsDir | Out-Null
}

# Install NSwag CLI if not installed
$nswagPath = "$toolsDir/nswag/nswag.exe"
if (-not (Test-Path $nswagPath)) {
    Write-Host "Installing NSwag CLI..."
    $nswagZipPath = "$toolsDir/nswag.zip"
    Invoke-WebRequest -Uri "https://github.com/RicoSuter/NSwag/releases/download/v13.20.0/NSwag.zip" -OutFile $nswagZipPath
    Expand-Archive -Path $nswagZipPath -DestinationPath "$toolsDir/nswag" -Force
    Remove-Item $nswagZipPath -Force
}

# Ensure the API is running to obtain the Swagger
$apiUrl = "http://localhost:5000"
$swaggerUrl = "$apiUrl/swagger/v1/swagger.json"
$swaggerOutputPath = "$toolsDir/swagger.json"

Write-Host "Ensure the API is running at $apiUrl"
Write-Host "Press any key when the API is ready..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Download the Swagger file
Write-Host "Downloading Swagger from $swaggerUrl..."
try {
    Invoke-WebRequest -Uri $swaggerUrl -OutFile $swaggerOutputPath
    Write-Host "Swagger downloaded successfully."
}
catch {
    Write-Host "Error downloading Swagger. Ensure the API is running." -ForegroundColor Red
    Write-Host $_.Exception.Message
    exit 1
}

# Generate integration tests for each endpoint
$testsOutputDir = "./tests/BookPetGroomingAPI.IntegrationTests/Endpoints"
if (-not (Test-Path $testsOutputDir)) {
    New-Item -ItemType Directory -Path $testsOutputDir | Out-Null
}

Write-Host "Analyzing Swagger to generate tests..."
$swagger = Get-Content $swaggerOutputPath | ConvertFrom-Json

# Create a test file per controller
foreach ($path in $swagger.paths.PSObject.Properties) {
    $endpoint = $path.Name
    $methods = $path.Value.PSObject.Properties | Where-Object { @("get", "post", "put", "delete", "patch") -contains $_.Name }
    
    foreach ($method in $methods) {
        $httpMethod = $method.Name.ToUpper()
        $operation = $method.Value
        $operationId = $operation.operationId
        $tags = $operation.tags
        
        if (-not $tags -or $tags.Count -eq 0) {
            $controllerName = "Misc"
        } else {
            $controllerName = $tags[0]
        }
        
        $testClassName = "${controllerName}EndpointTests"
        $testFilePath = "$testsOutputDir/$testClassName.cs"
        
        # Create or update the test file
        if (-not (Test-Path $testFilePath)) {
            # Create a new test file
            $testFileContent = @"
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using BookPetGroomingAPI.IntegrationTests.Fixtures;

namespace BookPetGroomingAPI.IntegrationTests.Endpoints;

public class ${testClassName} : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ${testClassName}(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    // Tests will be added here
}
"@
            Set-Content -Path $testFilePath -Value $testFileContent
        }
        
        # Read the existing file
        $existingContent = Get-Content -Path $testFilePath -Raw
        
        # Generate the test method
        $testMethodName = "${operationId}_ReturnsExpectedResponse"
        
        # Check if the test method already exists
        if ($existingContent -match "\s+public\s+async\s+Task\s+$testMethodName\s*\(") {
            Write-Host "The test for $operationId already exists in $testFilePath" -ForegroundColor Yellow
            continue
        }
        
        # Generate the test method code
        $testMethod = @"

    [Fact]
    public async Task ${testMethodName}()
    {
        // Arrange
        // TODO: Configure the necessary test data for this endpoint
        
        // Act
        var response = await _client.${httpMethod}Async("$endpoint");
        
        // Assert
        // TODO: Adjust assertions according to expected behavior
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        
        // Example of response content verification:
        // var content = await response.Content.ReadFromJsonAsync<YourResponseType>(_jsonOptions);
        // content.Should().NotBeNull();
    }
"@
        
        # Insert the test method before the last bracket
        $updatedContent = $existingContent -replace "(\s*\}\s*)$", "$testMethod`$1"
        Set-Content -Path $testFilePath -Value $updatedContent
        
        Write-Host "Generated test for $httpMethod $endpoint in $testFilePath" -ForegroundColor Green
    }
}

Write-Host "Test generation completed. Tests have been created in $testsOutputDir"
Write-Host "To run these tests with coverage, use the script run-coverage.ps1"