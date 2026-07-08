namespace FCG.NotificationsAPI.Contratos;

public record UserCreatedEvent(
    Guid     UsuarioId,
    string   Nome,
    string   Email,
    string   Role,
    DateTime CriadoEm);