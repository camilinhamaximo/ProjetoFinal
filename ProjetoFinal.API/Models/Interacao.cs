namespace ProjetoFinal.API.Models;

public sealed class Interacao
{
    public int Id { get; set; }
    public int ChamadoId { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTimeOffset DataRegistro { get; set; }
    public Chamado Chamado { get; set; } = null!;
}
