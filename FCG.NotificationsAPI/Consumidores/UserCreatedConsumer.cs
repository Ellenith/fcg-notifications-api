// ============================================================
// UserCreatedConsumer.cs — Consome UserCreatedEvent
//
// Simula o envio de e-mail de boas-vindas logando no console.
// Em produção: integrar com SendGrid, MailKit, etc.
// ============================================================

using FCG.NotificationsAPI.Contratos;
using MassTransit;

namespace FCG.NotificationsAPI.Consumidores;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var evento = context.Message;

        // Simula envio de e-mail de boas-vindas
        _logger.LogInformation(
            "📧 [E-MAIL BOAS-VINDAS SIMULADO]" +
            "\n  Para: {Email}" +
            "\n  Assunto: Bem-vindo à FIAP Cloud Games, {Nome}!" +
            "\n  Corpo: Olá {Nome}, sua conta foi criada com sucesso!" +
            "\n         Role: {Role} | Criado em: {CriadoEm}",
            evento.Email,
            evento.Nome,
            evento.Nome,
            evento.Role,
            evento.CriadoEm.ToLocalTime());

        return Task.CompletedTask;
    }
}