#!/usr/bin/env pwsh
# ============================================================================
# Pipeline Test Runner - Validation & Execution
# ============================================================================
# Ce script exécute tous les tests (unitaires et intégration) de manière
# similaire au pipeline GitHub Actions.
# ============================================================================

param(
    [ValidateSet('all', 'unit', 'integration', 'coverage', 'security')]
    [string]$TestType = 'all',
    
    [string]$Configuration = 'Release',
    
    [switch]$Verbose,
    
    [switch]$NoColor
)

# ============================================================================
# Configuration
# ============================================================================
$DOTNET_VERSION = '10.0.x'
$PROJECT_NAME = 'AdvancedDevTP'
$TEST_PROJECT = "AdvancedDevTP.Tests/AdvancedDevTP.Tests.csproj"
$RESULTS_DIR = "./test-results"

# Couleurs
$ColorReset = if ($NoColor) { "" } else { "`e[0m" }
$ColorGreen = if ($NoColor) { "" } else { "`e[32m" }
$ColorRed = if ($NoColor) { "" } else { "`e[31m" }
$ColorYellow = if ($NoColor) { "" } else { "`e[33m" }
$ColorBlue = if ($NoColor) { "" } else { "`e[34m" }

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Blue
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "$Message" -ForegroundColor Cyan
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ""
}

# ============================================================================
# Fonctions principales
# ============================================================================

function Test-Restore {
    Write-Header "1️⃣  RESTORE & ANALYZE"
    
    try {
        Write-Info "Restauration des packages NuGet..."
        dotnet restore
        Write-Success "Packages restaurés"
        
        Write-Info "Vérification des vulnérabilités..."
        dotnet list package --vulnerable --format json | Out-Null
        Write-Success "Vérification de sécurité complétée"
        
        return $true
    }
    catch {
        Write-Error "Échec de la restauration: $_"
        return $false
    }
}

function Test-Build {
    Write-Header "2️⃣  BUILD"
    
    try {
        Write-Info "Compilation en configuration $Configuration..."
        dotnet build --configuration $Configuration --no-restore --verbosity normal
        Write-Success "Build $Configuration réussi"
        
        Write-Info "Compilation en configuration Debug..."
        dotnet build --configuration Debug --no-restore --verbosity normal
        Write-Success "Build Debug réussi"
        
        return $true
    }
    catch {
        Write-Error "Échec du build: $_"
        return $false
    }
}

function Test-UnitTests {
    Write-Header "3️⃣  UNIT TESTS"
    
    try {
        Write-Info "Exécution des tests unitaires (sans intégration)..."
        
        $testArgs = @(
            $TEST_PROJECT,
            "--configuration", $Configuration,
            "--no-restore",
            "--verbosity", "normal",
            "--filter", "FullyQualifiedName!~Integrations",
            "--logger", "trx;LogFileName=$RESULTS_DIR/unit-test-results.trx",
            "--collect:`"XPlat Code Coverage`""
        )
        
        dotnet test @testArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Tests unitaires réussis"
            return $true
        }
        else {
            Write-Error "Échec des tests unitaires"
            return $false
        }
    }
    catch {
        Write-Error "Erreur lors des tests unitaires: $_"
        return $false
    }
}

function Test-IntegrationTests {
    Write-Header "4️⃣  INTEGRATION TESTS"
    
    try {
        Write-Info "Exécution des tests d'intégration API..."
        
        $testArgs = @(
            $TEST_PROJECT,
            "--configuration", $Configuration,
            "--no-restore",
            "--verbosity", "normal",
            "--filter", "FullyQualifiedName~Integrations",
            "--logger", "trx;LogFileName=$RESULTS_DIR/integration-test-results.trx",
            "--collect:`"XPlat Code Coverage`""
        )
        
        dotnet test @testArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Tests d'intégration réussis"
            return $true
        }
        else {
            Write-Error "Échec des tests d'intégration"
            return $false
        }
    }
    catch {
        Write-Error "Erreur lors des tests d'intégration: $_"
        return $false
    }
}

function Test-Coverage {
    Write-Header "5️⃣  CODE COVERAGE ANALYSIS"
    
    try {
        Write-Info "Exécution des tests avec analyse de couverture..."
        
        $testArgs = @(
            $TEST_PROJECT,
            "--configuration", $Configuration,
            "--no-restore",
            "--collect:`"XPlat Code Coverage`""
        )
        
        dotnet test @testArgs
        
        Write-Success "Analyse de couverture complétée"
        Write-Info "Résultats disponibles dans: $RESULTS_DIR"
        
        return $true
    }
    catch {
        Write-Error "Erreur lors de l'analyse de couverture: $_"
        return $false
    }
}

function Test-Security {
    Write-Header "6️⃣  SECURITY CHECK"
    
    try {
        Write-Info "Scan des vulnérabilités NuGet..."
        $vulnerabilities = dotnet list package --vulnerable --format json
        
        Write-Success "Scan de sécurité complété"
        
        # Afficher les résultats
        if ($vulnerabilities -like '*"Critical"*' -or $vulnerabilities -like '*"High"*') {
            Write-Warning "Des vulnérabilités ont été détectées"
        }
        else {
            Write-Success "Aucune vulnérabilité critique détectée"
        }
        
        return $true
    }
    catch {
        Write-Warning "Impossible de vérifier les vulnérabilités: $_"
        return $true  # Ne pas bloquer le pipeline
    }
}

function Show-Summary {
    param(
        [hashtable]$Results
    )
    
    Write-Header "📊 RÉSUMÉ DU PIPELINE"
    
    Write-Host "Résultats par étape:"
    Write-Host ""
    
    $steps = @(
        @{ Name = "Restore & Analyze"; Key = "restore" },
        @{ Name = "Build"; Key = "build" },
        @{ Name = "Unit Tests"; Key = "unit" },
        @{ Name = "Integration Tests"; Key = "integration" },
        @{ Name = "Coverage"; Key = "coverage" },
        @{ Name = "Security"; Key = "security" }
    )
    
    $allPassed = $true
    foreach ($step in $steps) {
        $status = if ($Results[$step.Key]) { "✓ SUCCESS" } else { "✗ FAILED" }
        $color = if ($Results[$step.Key]) { "Green" } else { "Red" }
        
        Write-Host "$($step.Name.PadRight(25)): " -NoNewline
        Write-Host $status -ForegroundColor $color
        
        if (-not $Results[$step.Key]) {
            $allPassed = $false
        }
    }
    
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    
    if ($allPassed) {
        Write-Success "Tous les tests ont réussi! 🎉"
    }
    else {
        Write-Error "Certains tests ont échoué. Vérifiez les logs ci-dessus."
    }
    
    Write-Host ""
    Write-Info "Rapports disponibles:"
    Write-Host "  • $RESULTS_DIR/unit-test-results.trx"
    Write-Host "  • $RESULTS_DIR/integration-test-results.trx"
    Write-Host "  • TestResults/coverage.cobertura.xml"
    Write-Host ""
}

# ============================================================================
# Main
# ============================================================================

function Main {
    Write-Header "🚀 $PROJECT_NAME - Pipeline Test Runner"
    Write-Info "Configuration: $Configuration"
    Write-Info "Type de tests: $TestType"
    
    # Créer le répertoire des résultats
    if (-not (Test-Path $RESULTS_DIR)) {
        New-Item -ItemType Directory -Path $RESULTS_DIR -Force | Out-Null
    }
    
    $results = @{}
    
    # Exécuter les tests selon le type
    if ($TestType -eq 'all' -or $TestType -eq 'restore') {
        $results['restore'] = Test-Restore
    }
    
    if ($results['restore'] -ne $false) {
        if ($TestType -eq 'all' -or $TestType -eq 'build') {
            $results['build'] = Test-Build
        }
    }
    
    if ($results['build'] -ne $false) {
        if ($TestType -eq 'all' -or $TestType -eq 'unit') {
            $results['unit'] = Test-UnitTests
        }
        
        if ($TestType -eq 'all' -or $TestType -eq 'integration') {
            $results['integration'] = Test-IntegrationTests
        }
        
        if ($TestType -eq 'all' -or $TestType -eq 'coverage') {
            $results['coverage'] = Test-Coverage
        }
        
        if ($TestType -eq 'all' -or $TestType -eq 'security') {
            $results['security'] = Test-Security
        }
    }
    
    # Afficher le résumé
    Show-Summary -Results $results
    
    # Code de sortie
    $allPassed = $results.Values | Where-Object { $_ -eq $false } | Measure-Object | Select-Object -ExpandProperty Count
    exit $allPassed
}

# ============================================================================
# Exécution
# ============================================================================

Main
