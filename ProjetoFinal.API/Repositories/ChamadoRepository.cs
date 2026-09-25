using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Repositories;

public sealed class ChamadoRepository(AppDbContext context) : IChamadoRepository
{
    public async Task<IReadOnlyList<Chamado>> ObterTodosComFiltrosAsync(
        StatusChamado? status,
        PrioridadeChamado? prioridade,
        int? categoriaId)
    {
        var consulta = context.Chamados
            .AsNoTracking()
            .Include(chamado => chamado.Categoria)
            .Include(chamado => chamado.Interacoes)
            .AsSplitQuery()
            .AsQueryable();

        if (status is not null)
        {
            consulta = consulta.Where(chamado => chamado.Status == status);
        }

        if (prioridade is not null)
        {
            consulta = consulta.Where(chamado => chamado.Prioridade == prioridade);
        }

        if (categoriaId is not null)
        {
            consulta = consulta.Where(chamado => chamado.CategoriaId == categoriaId);
        }

        return await consulta
            .OrderByDescending(chamado => chamado.DataAbertura)
            .ToListAsync();
    }

    public Task<Chamado?> ObterPorIdComDetalhesAsync(int id) =>
        context.Chamados
            .AsNoTracking()
            .Include(chamado => chamado.Categoria)
            .Include(chamado => chamado.Interacoes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(chamado => chamado.Id == id);

    public Task<Chamado?> ObterParaAtualizacaoAsync(int id) =>
        context.Chamados
            .Include(chamado => chamado.Interacoes)
            .FirstOrDefaultAsync(chamado => chamado.Id == id);

    public Task AdicionarAsync(Chamado chamado) => context.Chamados.AddAsync(chamado).AsTask();

    public Task AdicionarInteracaoAsync(Interacao interacao) => context.Interacoes.AddAsync(interacao).AsTask();

    public Task SalvarAlteracoesAsync() => context.SaveChangesAsync();
}
