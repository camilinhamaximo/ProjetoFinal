using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.API.DTOs.Categorias;

public sealed class CriarCategoriaRequest
{
    [Required, StringLength(200)]
    public string Nome { get; init; } = string.Empty;
}
