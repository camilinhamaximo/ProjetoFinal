using System;

namespace ProjetoFinal.API.Models
{
    public class Interacao
    {
        public int Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Relacionamento com Chamado
        public int ChamadoId { get; set; }
        public Chamado? Chamado { get; set; }
    }
}