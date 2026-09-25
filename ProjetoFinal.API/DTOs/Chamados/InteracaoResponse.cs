namespace ProjetoFinal.API.DTOs.Chamados;

public sealed record InteracaoResponse(
    int Id,
    int ChamadoId,
    string Autor,
    string Mensagem,
    DateTimeOffset DataRegistro);
