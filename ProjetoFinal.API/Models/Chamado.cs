using System;
using System.Collections.Generic;

namespace ProjetoFinal.API.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public PrioridadeChamada Prioridade { get; set; }
        public StatusChamado Status { get; set; } = StatusChamado.Aberto;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }

        // Relacionamento com Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // Relacionamento com Interações
        public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
    }
}