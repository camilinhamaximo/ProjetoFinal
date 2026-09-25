using ProjetoFinal.API.DTOs.Categorias;

namespace ProjetoFinal.API.DTOs.Chamados;

public sealed record ChamadoDetalheResponse(
    int Id,
    string Titulo,
    string Descricao,
    string Prioridade,
    string Status,
    string SolicitanteNome,
    DateTimeOffset DataAbertura,
    DateTimeOffset? DataFechamento,
    string? Solucao,
    int CategoriaId,
    CategoriaResponse Categoria,
    IReadOnlyList<InteracaoResponse> Interacoes);
