using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.API.DTOs.Chamados;

public sealed class EncerrarChamadoRequest
{
    [Required]
    public string Solucao { get; init; } = string.Empty;
}
