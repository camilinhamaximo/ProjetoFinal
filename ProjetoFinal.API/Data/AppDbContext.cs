using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Interacao> Interacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Impedir exclusão de Categoria se houver Chamados associados 
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Chamados)
                .WithOne(ch => ch.Categoria)
                .HasForeignKey(ch => ch.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Chamado x Interacao
            modelBuilder.Entity<Chamado>()
                .HasMany(ch => ch.Interacoes)
                .WithOne(i => i.Chamado)
                .HasForeignKey(i => i.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}