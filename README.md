# 🆕 StBurger

Teste prático para posição de desenvolvedor .NET.

## ❓ Requisitos

• Construir uma API REST em C# com .NET / ASP.NET Core.
• Implementar o CRUD completo de pedidos: criar, listar, consultar por identificador, atualizar e remover.
• Calcular corretamente o desconto, subtotal e total final de cada pedido, seguindo as regras acima.
• Validar erros e retornar respostas claras (ex.: itens duplicados, pedido inválido, recurso não encontrado).
• Expor também um endpoint para consultar o cardápio

- Regras de desconto:

• Sanduíche + batata + refrigerante → 20% de desconto
• Sanduíche + refrigerante → 15% de desconto
• Sanduíche + batata → 10% de desconto
• Cada pedido pode conter apenas um sanduíche, uma batata e um refrigerante. Itens duplicados devem
retornar uma mensagem de erro clara.

## ⚡ Decisões Técnicas

### 🧩 Patterns utilizados

- CQRS
- Mediator
- Decorator
- Repository
- Unit of Work

## 🔧 Construindo e executando

Antes de tudo, certifique-se que as portas 80 e 443 estão livres.

### 🔨 Construindo o projeto (dotnet cli)

Clone o repositório em sua pasta de desenvolvimento,
navegue até a raiz do repositório clonado e execute os seguintes comandos:

```bash
cd .\src\StBurger.App\
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=stburger;Trusted_Connection=True;MultipleActiveResultSets=true"
dotnet restore 
dotnet build 
dotnet run 
```

### ▶ Construindo o projeto (docker)

Após clonar o repositório e navegar para a raíz do mesmo, siga os seguintes passos:

```bash

cp .\.env.defaults .\.env

```

no powershell como administrador, executar o seguinte script,
provendo os valores conforme necessário de acordo com os dados do arquivo `.env`: 

```bash
.\scripts\gen-certs.ps1
```

Em seguida, subir os containers separadamente, esperando o container do sqlserver provisionar o user antes de subir o container da aplicação:

```bash
docker-compose up -d sqlserver
docker-compose up app
```

## 🎯 Extras e TODO's

Extras:

- Unit tests

Todo's

- Frontend
