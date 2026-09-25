using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Chamado> Chamados => Set<Chamado>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Interacao> Interacoes => Set<Interacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(categoria => categoria.Id);
            entity.Property(categoria => categoria.Nome)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_Chamados_Prioridade", "[Prioridade] IN (1, 2, 3)");
                table.HasCheckConstraint("CK_Chamados_Status", "[Status] IN (1, 2, 3)");
            });

            entity.HasKey(chamado => chamado.Id);
            entity.Property(chamado => chamado.Titulo)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(chamado => chamado.Descricao)
                .IsRequired();
            entity.Property(chamado => chamado.Prioridade)
                .HasConversion<int>();
            entity.Property(chamado => chamado.Status)
                .HasConversion<int>();
            entity.Property(chamado => chamado.SolicitanteNome)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(chamado => chamado.DataAbertura)
                .HasColumnType("datetimeoffset");
            entity.Property(chamado => chamado.DataFechamento)
                .HasColumnType("datetimeoffset");
            entity.Property(chamado => chamado.Solucao);

            entity.HasOne(chamado => chamado.Categoria)
                .WithMany(categoria => categoria.Chamados)
                .HasForeignKey(chamado => chamado.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Interacao>(entity =>
        {
            entity.ToTable("Interacoes");
            entity.HasKey(interacao => interacao.Id);
            entity.Property(interacao => interacao.Autor)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(interacao => interacao.Mensagem)
                .IsRequired();
            entity.Property(interacao => interacao.DataRegistro)
                .HasColumnType("datetimeoffset");

            entity.HasOne(interacao => interacao.Chamado)
                .WithMany(chamado => chamado.Interacoes)
                .HasForeignKey(interacao => interacao.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
