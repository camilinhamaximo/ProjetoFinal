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

            // Validações e Relacionamentos
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Chamado>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Descricao).IsRequired();
                entity.HasOne(c => c.Categoria)
                      .WithMany(cat => cat.Chamados)
                      .HasForeignKey(c => c.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Interacao>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Mensagem).IsRequired();
                entity.HasOne(i => i.Chamado)
                      .WithMany(ch => ch.Interacoes)
                      .HasForeignKey(i => i.ChamadoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}