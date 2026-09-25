using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Repositories;

public sealed class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    public async Task<IReadOnlyList<Categoria>> ObterTodasAsync() =>
        await context.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nome)
            .ToListAsync();

    public Task<Categoria?> ObterPorIdAsync(int id) =>
        context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(categoria => categoria.Id == id);

    public Task<Categoria?> ObterParaAtualizacaoAsync(int id) =>
        context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);

    public Task<bool> PossuiChamadosAsync(int categoriaId) =>
        context.Chamados.AnyAsync(chamado => chamado.CategoriaId == categoriaId);

    public Task AdicionarAsync(Categoria categoria) =>
        context.Categorias.AddAsync(categoria).AsTask();

    public void Remover(Categoria categoria) => context.Categorias.Remove(categoria);

    public Task SalvarAlteracoesAsync() => context.SaveChangesAsync();
}
