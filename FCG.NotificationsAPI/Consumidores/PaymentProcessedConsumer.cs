// ============================================================
// PaymentProcessedConsumer.cs — Consome PaymentProcessedEvent
//
// Simula o envio de e-mail de confirmação ou rejeição de compra.
// ============================================================

using FCG.Contratos;
using MassTransit;

namespace FCG.NotificationsAPI.Consumidores;

public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedConsumer> _logger;

    public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var evento = context.Message;

        if (evento.Status == "Approved")
        {
            // Simula e-mail de confirmação de compra
            _logger.LogInformation(
                "📧 [E-MAIL CONFIRMAÇÃO DE COMPRA SIMULADO]" +
                "\n  Para: UsuarioId={UsuarioId}" +
                "\n  Assunto: Sua compra foi aprovada!" +
                "\n  Corpo: O jogo JogoId={JogoId} foi adicionado à sua biblioteca." +
                "\n         Valor pago: R${Preco:F2}" +
                "\n         OrderId: {OrderId}",
                evento.UsuarioId,
                evento.JogoId,
                evento.Preco,
                evento.OrderId);
        }
        else
        {
            // Simula e-mail de rejeição
            _logger.LogWarning(
                "📧 [E-MAIL PAGAMENTO REJEITADO SIMULADO]" +
                "\n  Para: UsuarioId={UsuarioId}" +
                "\n  Assunto: Problema com seu pagamento" +
                "\n  Corpo: Infelizmente seu pagamento foi recusado." +
                "\n         Motivo: {Mensagem}" +
                "\n         OrderId: {OrderId}",
                evento.UsuarioId,
                evento.Mensagem,
                evento.OrderId);
        }

        return Task.CompletedTask;
    }
}