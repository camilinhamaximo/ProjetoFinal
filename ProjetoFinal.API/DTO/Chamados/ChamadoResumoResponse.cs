namespace ProjetoFinal.API.DTOs.Chamados;

public sealed record ChamadoResumoResponse(
    int Id,
    string Titulo,
    string Prioridade,
    string Status,
    DateTimeOffset DataAbertura,
    int CategoriaId,
    string CategoriaNome);
