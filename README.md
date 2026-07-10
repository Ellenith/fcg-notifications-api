
# FIAP Cloud Gaming - Fase 02

Tech Challenge Pos-graduacao FIAP.

# FCG — Notifications API

Microsserviço de Notificações da FIAP Cloud Games. Responsável por
simular o envio de e-mails de boas-vindas e de confirmação de compra.

Parte da Fase 2 do Tech Challenge — arquitetura de microsserviços
orientada a eventos.

---

## Responsabilidades

- Consome `UserCreatedEvent` e simula o envio de e-mail de boas-vindas
- Consome `PaymentProcessedEvent` e simula o envio de e-mail de
  confirmação (se aprovado) ou de aviso de recusa (se rejeitado)

---

## Características

Assim como o `PaymentsAPI`, este serviço **não possui banco de dados
e não expõe endpoints de negócio**. O envio de e-mail é simulado via
log estruturado no console — não há integração real com nenhum
provedor de e-mail nesta fase.

---

## Tecnologias

- .NET 9
- MassTransit + RabbitMQ (consome eventos)
- Serilog (logs em arquivo)

---

## Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/health` | Health check para o Kubernetes |

---

## Eventos consumidos

### `UserCreatedEvent`

```csharp
public record UserCreatedEvent(
    Guid     UsuarioId,
    string   Nome,
    string   Email,
    string   Role,
    DateTime CriadoEm);
```

Ação: loga uma simulação de e-mail de boas-vindas contendo nome,
e-mail e role do novo usuário.

### `PaymentProcessedEvent`

```csharp
public record PaymentProcessedEvent(
    Guid     OrderId,
    Guid     UsuarioId,
    Guid     JogoId,
    decimal  Preco,
    string   Status,
    string   Mensagem,
    DateTime ProcessadoEm);
```

Ação:
- Se `Status == "Approved"` → loga simulação de e-mail de confirmação
  de compra
- Se `Status == "Rejected"` → loga simulação de e-mail avisando sobre
  o pagamento recusado

---

## Variáveis de ambiente

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `RabbitMQ__Host` | Host do RabbitMQ | `rabbitmq` (Docker) ou `localhost` |
| `RabbitMQ__Usuario` | Usuário do RabbitMQ | `guest` |
| `RabbitMQ__Senha` | Senha do RabbitMQ | `guest` |

---

## Como rodar localmente

**Pré-requisitos:**
- .NET 9 SDK
- RabbitMQ rodando (local ou via Docker)

```bash
dotnet restore
dotnet run --project FCG.NotificationsAPI
```

Verifique se está rodando acessando `http://localhost:{porta}/health`.

---

## Como rodar com Docker

```bash
docker build -t fcg-notifications-api .
docker run -p 5004:8080 \
  -e RabbitMQ__Host=host.docker.internal \
  fcg-notifications-api
```

---

## Observando as notificações simuladas

```bash
docker logs fcg-notifications-api -f
```

Exemplo de saída esperada:

    📧 [E-MAIL BOAS-VINDAS SIMULADO]
    Para: lucas@fiap.com.br
    Assunto: Bem-vindo à FIAP Cloud Games, Lucas Silva!
    Corpo: Olá Lucas Silva, sua conta foi criada com sucesso!
    Role: Admin | Criado em: 03/07/2026 10:00:00

---

*FIAP Cloud Games · Tech Challenge Fase 2 · PosTech FIAP*