using System.Text.Json.Serialization;

namespace ProjetoFinal.API.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        // Enum de Status e Prioridade
        public StatusChamado Status { get; set; } = StatusChamado.Aberto;
        public PrioridadeChamado Prioridade { get; set; } = PrioridadeChamado.Media;

        // Chave Estrangeira e Navegação para Categoria
        public int CategoriaId { get; set; }
        
        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        // Relacionamento 1:N com Interações
        public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
    }

    public enum StatusChamado
    {
        Aberto = 1,
        EmAndamento = 2,
        Resolvido = 3,
        Fechado = 4
    }

    public enum PrioridadeChamado
    {
        Baixa = 1,
        Media = 2,
        Alta = 3,
        Urgente = 4
    }
}