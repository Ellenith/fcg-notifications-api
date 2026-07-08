namespace FCG.Contratos;

public record UserCreatedEvent(
    Guid     UsuarioId,
    string   Nome,
    string   Email,
    string   Role,
    DateTime CriadoEm);