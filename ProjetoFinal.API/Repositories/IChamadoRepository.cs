using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Repositories;

public interface IChamadoRepository
{
    Task<IReadOnlyList<Chamado>> ObterTodosComFiltrosAsync(
        StatusChamado? status,
        PrioridadeChamado? prioridade,
        int? categoriaId);
    Task<Chamado?> ObterPorIdComDetalhesAsync(int id);
    Task<Chamado?> ObterParaAtualizacaoAsync(int id);
    Task AdicionarAsync(Chamado chamado);
    Task AdicionarInteracaoAsync(Interacao interacao);
    Task SalvarAlteracoesAsync();
}
