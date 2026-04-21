Param(
    [Parameter(Mandatory=$true)]
    [Alias("m", "MN")]
    [ValidateNotNullOrEmpty()] 
    [string]$MigrationName, 
    [switch]$v
)
 
$migrationsPath = "$PSScriptRoot/../src/StBurger.Infrastructure/Migrations"
 
if (Test-Path "$migrationsPath/*_$MigrationName.cs") {
    Write-Error "Erro: A migration '$MigrationName' já existe em $migrationsPath."
    return  
}
 
Write-Host "Iniciando a criação da migration: $MigrationName..." -ForegroundColor Blue

if ($v.IsPresent) {
    dotnet ef migrations add $MigrationName `
        -p $PSScriptRoot/../src/StBurger.Infrastructure/StBurger.Infrastructure.csproj `
        -s $PSScriptRoot/../src/StBurger.App/StBurger.App.csproj `
        -c StBurgerDbContext -v
} else {
    dotnet ef migrations add $MigrationName `
        -p $PSScriptRoot/../src/StBurger.Infrastructure/StBurger.Infrastructure.csproj `
        -s $PSScriptRoot/../src/StBurger.App/StBurger.App.csproj `
        -c StBurgerDbContext 
}