# ProjetoRifa

API REST para gerenciamento de rifas/sorteios em **C# / ASP.NET Core (.NET 10)** com persistência em SQLite.

## Equipe
- Guilherme Maia Arriel (324112890)
- Samuel Soares (324147097)
- Marlon Emanuel (324149349)
- Bernardo Araujo (324112780)

## Como executar
```Powershell Mais recomendado
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run 
http://localhost:5000/swagger
```

```bash
dotnet run
```

O banco `rifa.db` e as tabelas sao criados automaticamente.
Swagger disponivel em `http://localhost:5000/swagger` (apenas em Development).

## Endpoints

| Metodo | Rota | Descricao |
|--------|------|-----------|
| POST | `/api/raffles` | Criar rifa |
| GET | `/api/raffles/{id}` | Consultar rifa com bilhetes |
| POST | `/api/raffles/{id}/tickets` | Registrar bilhete |
| POST | `/api/raffles/{id}/draw` | Realizar sorteio |
| GET | `/api/raffles/{id}/report` | Gerar relatorio |
| DELETE | `/api/raffles/{id}` | Deletar rifa |

## Exemplos curl

```bash
# Criar rifa
curl -X POST http://localhost:5000/api/raffles \
  -H "Content-Type: application/json" \
  -d '{"title": "Rifa de Natal"}'

# Registrar bilhete
curl -X POST http://localhost:5000/api/raffles/1/tickets \
  -H "Content-Type: application/json" \
  -d '{"buyerName": "Joao Silva", "number": 42}'

# Sortear
curl -X POST http://localhost:5000/api/raffles/1/draw \
  -H "Content-Type: application/json" \
  -d '{}'

# Relatorio
curl http://localhost:5000/api/raffles/1/report

# Deletar
curl -X DELETE http://localhost:5000/api/raffles/1
```

## Codigos HTTP

| Codigo | Situacao |
|--------|----------|
| 200 | Sucesso |
| 201 | Criado com sucesso |
| 204 | Deletado com sucesso |
| 400 | Dados invalidos |
| 404 | Nao encontrado |
| 409 | Numero de bilhete duplicado |
