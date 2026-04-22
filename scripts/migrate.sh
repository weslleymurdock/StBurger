!#/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
MIGRATIONS_DIR="$SCRIPT_DIR/../src/StBurger.Infrastructure/Persistence/Migrations"
INFRA_PROJ="$SCRIPT_DIR/../src/StBurger.Infrastructure/StBurger.Infrastructure.csproj"
APP_PROJ="$SCRIPT_DIR/../src/StBurger.App/StBurger.App.csproj"
MigrationName=$1
is_verbose=$2

if ls "$migrationsPath"/*_"$MigrationName".cs >/dev/null 2>&1; then
    echo "Erro: A migration '$MigrationName' já existe em $migrationsPath." >&2
    exit 1
fi

echo -e "\e[34mIniciando a criação da migration: $MigrationName...\e[0m"

ARGS=(migrations add "$MigrationName" -p "$INFRA_PROJ" -s "$APP_PROJ" -c StBurgerDbContext)

# Adiciona -v se for verbose
if [ "$is_verbose" = true ]; then
    ARGS+=("-v")
fi

dotnet ef "${ARGS[@]}"