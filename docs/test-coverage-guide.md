# Test Coverage Guide for BookPetGroomingAPI

## Introduction

This document describes how to execute and visualize test coverage for the BookPetGroomingAPI project. Test coverage is a metric that indicates what percentage of the source code is being tested by automated tests.

## Prerequisites

- .NET SDK 6.0 or higher
- PowerShell
- Visual Studio Code (optional, for in-editor visualization)
  - [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters) extension

## Running Test Coverage

### Using PowerShell

1. Open a PowerShell terminal at the root of the project
2. Execute the coverage script:

```powershell
.\run-coverage.ps1
```

This script:
- Installs ReportGenerator if not installed
- Runs unit and integration tests with coverage
- Generates detailed HTML reports
- Automatically opens the report in your default browser

### Using Visual Studio Code

1. Open the project in VS Code
2. Press `Ctrl+Shift+P` to open the command palette
3. Type "Tasks: Run Task" and select this option
4. Select "Run Test Coverage"

## Viewing Coverage

### HTML Report

After running the script, an HTML report will automatically open in your browser. This report includes:

- General coverage summary
- Breakdown by project/assembly
- Breakdown by class and method
- Source code visualization with covered/uncovered lines

The report is saved in `coverage-reports/html/index.html`.

### In Visual Studio Code

If you have the Coverage Gutters extension installed:

1. Open any source code file in the project
2. Press `Ctrl+Shift+P` and search for "Coverage Gutters: Watch"
3. You will see indicators in the left margin:
   - Green: Line covered by tests
   - Red: Line not covered by tests
   - Yellow: Partially covered line

## Interpreting Results

### Key Metrics

- **Line Coverage**: Percentage of code lines executed during tests
- **Branch Coverage**: Percentage of code branches (if/else, switch, etc.) executed
- **Method Coverage**: Percentage of methods that have been called during tests


## Custom Configuration

### Exclusions

The `coverage.runsettings` file is configured to exclude:

- Automatically generated code
- Database migrations
- Simple models

To modify these exclusions, edit the `coverage.runsettings` file.

### Continuous Integration

To integrate test coverage into a CI/CD pipeline, the `run-coverage.ps1` script is adapted to generate reports in XML or JSON format that can be consumed by tools like SonarQube, Azure DevOps, or GitHub Actions.

## Troubleshooting

### Report Not Generated

If the report is not generated correctly:

1. Verify that all tests run without errors
2. Ensure ReportGenerator is installed:
```
dotnet tool install -g dotnet-reportgenerator-globaltool
```
3. Check write permissions in the folder
```
coverage-reports
```

### Incorrect Coverage

If the coverage seems incorrect:

1. Clean the solution:
```
dotnet clean
```
2. Rebuild:
```
dotnet build
```
3. Delete the `coverage-reports` folder and rerun the script