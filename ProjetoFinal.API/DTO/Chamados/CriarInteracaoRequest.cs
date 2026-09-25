using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.API.DTOs.Chamados;

public sealed class CriarInteracaoRequest
{
    [Required, StringLength(200)]
    public string Autor { get; init; } = string.Empty;

    [Required]
    public string Mensagem { get; init; } = string.Empty;
}
