using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Repositories;

public interface ICategoriaRepository
{
    Task<IReadOnlyList<Categoria>> ObterTodasAsync();
    Task<Categoria?> ObterPorIdAsync(int id);
    Task<Categoria?> ObterParaAtualizacaoAsync(int id);
    Task<bool> PossuiChamadosAsync(int categoriaId);
    Task AdicionarAsync(Categoria categoria);
    void Remover(Categoria categoria);
    Task SalvarAlteracoesAsync();
}
