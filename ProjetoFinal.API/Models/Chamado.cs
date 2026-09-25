namespace ProjetoFinal.API.Models;

public sealed class Chamado
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public PrioridadeChamado Prioridade { get; set; } = PrioridadeChamado.Media;
    public StatusChamado Status { get; set; } = StatusChamado.Aberto;
    public string SolicitanteNome { get; set; } = string.Empty;
    public DateTimeOffset DataAbertura { get; set; }
    public DateTimeOffset? DataFechamento { get; set; }
    public string? Solucao { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
}

public enum StatusChamado
{
    Aberto = 1,
    EmAndamento = 2,
    Fechado = 3
}

public enum PrioridadeChamado
{
    Baixa = 1,
    Media = 2,
    Alta = 3
}
