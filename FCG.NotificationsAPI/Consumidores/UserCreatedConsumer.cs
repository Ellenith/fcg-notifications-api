using FCG.Contratos;
using MassTransit;

namespace FCG.NotificationsAPI.Consumidores;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var evento = context.Message;
        var payload = new { email = evento.Email, nome = evento.Nome, role = evento.Role, criadoEm = evento.CriadoEm };
        var url = _configuration["NotificationsFunction:UserCreatedUrl"];

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Evento UserCreated encaminhado pra Function: {Email}", evento.Email);
    }
}