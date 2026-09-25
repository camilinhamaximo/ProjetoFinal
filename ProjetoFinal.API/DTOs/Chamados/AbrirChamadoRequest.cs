using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.API.DTOs.Chamados;

public sealed class AbrirChamadoRequest
{
    [Required, StringLength(200)]
    public string Titulo { get; init; } = string.Empty;

    [Required]
    public string Descricao { get; init; } = string.Empty;

    [Required]
    public PrioridadePublica? Prioridade { get; init; }

    [Required, StringLength(200)]
    public string SolicitanteNome { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; init; }
}
