// ============================================================
// Program.cs — NotificationsAPI
// Microsserviço de Notificações da FIAP Cloud Games
// Sem endpoints HTTP — apenas consome eventos e loga notificações
// ============================================================

using FCG.NotificationsAPI.Consumidores;
using MassTransit;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/notifications-api-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddMassTransit(x =>
{
    // Registra os dois consumidores
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<PaymentProcessedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Usuario"]!);
            h.Password(builder.Configuration["RabbitMQ:Senha"]!);
        });

        cfg.ReceiveEndpoint("notifications-user-created", e =>
        {
            e.ConfigureConsumer<UserCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("notifications-payment-processed", e =>
        {
            e.ConfigureConsumer<PaymentProcessedConsumer>(context);
        });
    });
});

var app = builder.Build();

// Health check para o Kubernetes
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Service = "NotificationsAPI" }));

app.Run();