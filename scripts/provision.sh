#!/bin/bash
set -e
set +H

DBHOST="${DBHOST:-localhost}"  # fallback se não estiver definido
export PATH=$PATH:/opt/mssql-tools18/bin/
echo "🔄 Aguardando SQL '$DBHOST' aceitar conexões..."
for i in {1..30}; do
  sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}"  -C -Q "SELECT 1" && break
  sleep 2
done

echo "🛠️ Provisionando banco de dados '${DBNAME}'..."

sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}"  -C -Q "
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'${DBNAME}')
BEGIN
    CREATE DATABASE [${DBNAME}];
    PRINT '✔️ Banco criado.';
END
ELSE
BEGIN
    PRINT 'ℹ️ Banco já existe.';
END
"

echo "⏳ Aguardando banco '${DBNAME}' estar acessível..."
for i in {1..30}; do
  sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}" -d "${DBNAME}"  -C -Q "SELECT 1" && break
  sleep 2
done

echo "👤 Provisionando login '${DBUSER}'..."

sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}"  -C -Q "
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = N'${DBUSER}')
BEGIN
    CREATE LOGIN [${DBUSER}]
    WITH PASSWORD = N'${DBUSER_PASSWORD}', CHECK_POLICY = OFF;
    PRINT '✔️ Login criado.';
END
ELSE
BEGIN
    PRINT 'ℹ️ Login já existe.';
END
"

echo "📌 Provisionando usuário no banco '${DBNAME}'..."

sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}" -d "${DBNAME}"  -C -Q "
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'${DBUSER}')
BEGIN
    CREATE USER [${DBUSER}] FOR LOGIN [${DBUSER}];
    PRINT '✔️ Usuário criado.';
END
ELSE
BEGIN
    PRINT 'ℹ️ Usuário já existe.';
END
"

echo "🔐 Concedendo db_owner se necessário..."

sqlcmd -S "$DBHOST" -U sa -P "${SA_PASSWORD}" -d "${DBNAME}" -C -Q "
IF NOT EXISTS (
    SELECT dp.name
    FROM sys.database_principals dp
    JOIN sys.database_role_members drm ON dp.principal_id = drm.member_principal_id
    JOIN sys.database_principals roles ON drm.role_principal_id = roles.principal_id
    WHERE dp.name = N'${DBUSER}' AND roles.name = 'db_owner'
)
BEGIN
    EXEC sp_addrolemember N'db_owner', N'${DBUSER}';
    PRINT '✔️ Permissão db_owner atribuída.';
END
ELSE
BEGIN
    PRINT 'ℹ️ Usuário já possui db_owner.';
END
"
# 🧩 Executa script do Quartz se variável estiver definida
if [ "${QUARTZ_PROVISIONING}" = "true" ]; then
  echo "📅 Executando script de schema do Quartz..."
  sqlcmd -S "$DBHOST" -U ${DBUSER} -P "${DBUSER_PASSWORD}" -d "${DBNAME}" -C -i tables_sqlServer.sql
  echo "✔️ Script de schema do Quartz executado com sucesso."
fi


echo "✅ Provisionamento concluído com sucesso!"