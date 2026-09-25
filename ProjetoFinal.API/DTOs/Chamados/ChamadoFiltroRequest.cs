using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.API.DTOs.Chamados;

public sealed class ChamadoFiltroRequest
{
    public string? Status { get; init; }
    public string? Prioridade { get; init; }

    [Range(1, int.MaxValue)]
    public int? CategoriaId { get; init; }
}
