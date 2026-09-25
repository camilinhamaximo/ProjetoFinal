using ProjetoFinal.API.DTOs.Categorias;
using ProjetoFinal.API.Exceptions;
using ProjetoFinal.API.Models;
using ProjetoFinal.API.Repositories;

namespace ProjetoFinal.API.Services;

public interface ICategoriaService
{
    Task<IReadOnlyList<CategoriaResponse>> ObterTodasAsync();
    Task<CategoriaResponse> ObterPorIdAsync(int id);
    Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request);
    Task AtualizarAsync(int id, AtualizarCategoriaRequest request);
    Task DeletarAsync(int id);
}

public sealed class CategoriaService(ICategoriaRepository repository) : ICategoriaService
{
    public async Task<IReadOnlyList<CategoriaResponse>> ObterTodasAsync()
    {
        var categorias = await repository.ObterTodasAsync();
        return categorias.Select(ParaResponse).ToList();
    }

    public async Task<CategoriaResponse> ObterPorIdAsync(int id)
    {
        ValidarId(id);
        var categoria = await repository.ObterPorIdAsync(id)
            ?? throw new ResourceNotFoundException("Categoria não encontrada.");

        return ParaResponse(categoria);
    }

    public async Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request)
    {
        var categoria = new Categoria { Nome = ValidarNome(request.Nome) };

        await repository.AdicionarAsync(categoria);
        await repository.SalvarAlteracoesAsync();
        return ParaResponse(categoria);
    }

    public async Task AtualizarAsync(int id, AtualizarCategoriaRequest request)
    {
        ValidarId(id);
        var categoriaExistente = await repository.ObterParaAtualizacaoAsync(id)
            ?? throw new ResourceNotFoundException("Categoria não encontrada.");

        categoriaExistente.Nome = ValidarNome(request.Nome);
        await repository.SalvarAlteracoesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        ValidarId(id);
        var categoria = await repository.ObterParaAtualizacaoAsync(id)
            ?? throw new ResourceNotFoundException("Categoria não encontrada.");

        if (await repository.PossuiChamadosAsync(id))
        {
            throw new BusinessRuleViolationException("Não é possível excluir uma categoria com chamados associados.");
        }

        repository.Remover(categoria);
        await repository.SalvarAlteracoesAsync();
    }

    private static string ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new RequestValidationException("O nome da categoria é obrigatório.");
        }

        return nome.Trim();
    }

    private static void ValidarId(int id)
    {
        if (id <= 0)
        {
            throw new RequestValidationException("O identificador da categoria deve ser positivo.");
        }
    }

    private static CategoriaResponse ParaResponse(Categoria categoria) =>
        new(categoria.Id, categoria.Nome);
}
