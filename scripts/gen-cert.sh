#!/bin/bash

if [[ $EUID -ne 0 ]]; then
   echo "Este script precisa de acesso root. Solicitando sudo..."
   exec sudo "$0" "$@"
   exit
fi

read -p "Senha do PFX: " PASSWORD
read -p "Anos: " YEARS
read -p "Email: " EMAIL
read -p "IP Externo: " IP

FILENAME="stburger"
DAYS=$((YEARS * 365))
CERT_DIR="/usr/local/share/ca-certificates"
SCRIPT_DIR=$(dirname "$(realpath "$0")")
CERT_GEN_DIR="$SCRIPT_DIR/../certs"

echo "Limpando arquivos e certificados antigos..."

rm -f "$CERT_GEN_DIR/$FILENAME.pfx" "$CERT_GEN_DIR/$$FILENAME.crt" "$CERT_GEN_DIR/$$FILENAME.key"
rm -f "$CERT_DIR/$FILENAME.crt"

cat > openssl.conf <<EOF
[req]
distinguished_name = req_distinguished_name
x509_extensions = v3_req
prompt = no
[req_distinguished_name]
CN = localhost
emailAddress = $EMAIL
[v3_req]
keyUsage = critical, digitalSignature, keyEncipherment
extendedKeyUsage = serverAuth
subjectAltName = @alt_names
[alt_names]
DNS.1 = localhost
IP.1 = $IP
EOF

openssl req -x509 -nodes -days $DAYS -newkey rsa:2048 \
    -keyout "$CERT_GEN_DIR/$FILENAME.key" -out "$CERT_GEN_DIR/$FILENAME.crt" \
    -config openssl.conf -extensions v3_req

openssl pkcs12 -export -out "$CERT_GEN_DIR/$FILENAME.pfx" \
    -inkey "$CERT_GEN_DIR/$FILENAME.key" -in "$CERT_GEN_DIR/$FILENAME.crt" \
    -password "pass:$PASSWORD"

cp "$CERT_GEN_DIR/$FILENAME.crt" "$CERT_DIR/$FILENAME.crt"
update-ca-certificates

rm openssl.conf "$CERT_GEN_DIR/$FILENAME.key"
chmod 644 "$CERT_GEN_DIR/$FILENAME.pfx"
chmod 644 "$CERT_DIR/$FILENAME.pfx"

echo "Sucesso! Certificado gerado e sistema atualizado."