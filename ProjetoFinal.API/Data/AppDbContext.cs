using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Chamado> Chamados => Set<Chamado>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Interacao> Interacoes => Set<Interacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamento Chamado -> Categoria
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Categoria)
                .WithMany(cat => cat.Chamados)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mapeamento Interacao -> Chamado
            modelBuilder.Entity<Interacao>()
                .HasOne(i => i.Chamado)
                .WithMany(c => c.Interacoes)
                .HasForeignKey(i => i.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}