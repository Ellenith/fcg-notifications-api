using FCG.Contratos;
using MassTransit;

namespace FCG.NotificationsAPI.Consumidores;

public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedConsumer> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var evento = context.Message;
        var payload = new
        {
            usuarioId = evento.UsuarioId, jogoId = evento.JogoId, preco = evento.Preco,
            orderId = evento.OrderId, status = evento.Status, mensagem = evento.Mensagem
        };
        var url = _configuration["NotificationsFunction:PaymentProcessedUrl"];

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Evento PaymentProcessed encaminhado pra Function: {OrderId}", evento.OrderId);
    }
}