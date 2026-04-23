


dotnet ef database drop --force `
        -p $PSScriptRoot/../src/StBurger.Infrastructure/StBurger.Infrastructure.csproj `
        -s $PSScriptRoot/../src/StBurger.App/StBurger.App.csproj `
        -c StBurgerDbContext | Out-Null


$migrationsDir = "$PSScriptRoot/../src/StBurger.Infrastructure/Migrations"

if (Test-Path $migrationsDir) {
    Write-Host "Removendo migrações em $migrationsDir..." -ForegroundColor Yellow

    Remove-Item $migrationsDir -Recurse -Force

    Write-Host "Migrações removidas com sucesso." -ForegroundColor Green
} else {
    Write-Host "Não existem migrações a serem removidas." -ForegroundColor DarkGray
}

pause