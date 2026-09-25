using System.Text.Json.Serialization;

namespace ProjetoFinal.API.Models
{
    public class Interacao
    {
        public int Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Chave Estrangeira para Chamado
        public int ChamadoId { get; set; }
        
        [JsonIgnore]
        public Chamado? Chamado { get; set; }
    }
}