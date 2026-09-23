namespace ProjetoFinal.API.Models.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        // Relacionamento 1:N com Chamados
        public ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
    }
}