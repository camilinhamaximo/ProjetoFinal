namespace ProjetoFinal.API.Models.Entities;

public class Chamado
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public PrioridadeChamado Prioridade { get; set; }
    public StatusChamado Status { get; set; } = StatusChamado.Aberto;
    public string SolicitanteNome { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataFechamento { get; set; }
    public string? Solucao { get; set; }

    // Chave Estrangeira e Navegação para Categoria (1:N)
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    // Relacionamento 1:N com Interacao
    public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
}