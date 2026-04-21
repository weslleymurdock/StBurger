# --- Início da Elevação Automática ---
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "Solicitando acesso de administrador..." -ForegroundColor Cyan
    $arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""
    Start-Process powershell.exe -ArgumentList $arguments -Verb RunAs
    exit
}
# --- Fim da Elevação ---

$filename = "stburger"
$password = Read-Host "Senha do PFX"
$years = Read-Host "Expira em x anos"
$email = Read-Host "Email"
$ip = Read-Host "IP Externo do Host"

$certSubject = "CN=localhost"
$friendlyName = "Docker_Dev_$filename"

# 1. Limpeza de certificados antigos
Write-Host "Limpando certificados antigos ($friendlyName)..." -ForegroundColor Yellow
Get-ChildItem -Path Cert:\CurrentUser\My, Cert:\LocalMachine\Root | 
    Where-Object { $_.FriendlyName -eq $friendlyName -or $_.Subject -eq "CN=localhost" } | 
    Remove-Item -ErrorAction SilentlyContinue

# 2. Geração do Certificado com SAN (Subject Alternative Name)
$expiryDate = (Get-Date).AddYears([int]$years)
$securePassword = ConvertTo-SecureString $password -AsPlainText -Force

$cert = New-SelfSignedCertificate -DnsName "localhost", $ip `
    -CertStoreLocation "Cert:\CurrentUser\My" `
    -FriendlyName $friendlyName `
    -NotAfter $expiryDate `
    -Subject $certSubject `
    -Type Custom `
    -KeyUsage DigitalSignature, KeyEncipherment `
    -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1")

# 3. Exportação para PFX
$pfxDir = "$PSScriptRoot\..\certs"
$pfxPath = "$pfxDir\$filename.pfx"


if (-Not (Test-Path -Path $pfxDir)) {
    New-Item -ItemType Directory -Path $pfxDir
    echo "" > "$pfxDir\.gitkeep"
}

Export-PfxCertificate -Cert $cert -FilePath $pfxPath -Password $securePassword
 
Write-Host "Instalando na Root Store da Máquina Local..." -ForegroundColor Green
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store "Root", "LocalMachine"
$store.Open("ReadWrite")
$store.Add($cert)
$store.Close()

Write-Host "Concluído! O arquivo $filename.pfx está pronto." -ForegroundColor Cyan
pause