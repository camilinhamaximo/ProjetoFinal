using ProjetoFinal.API.DTOs.Categorias;
using ProjetoFinal.API.DTOs.Chamados;
using ProjetoFinal.API.Exceptions;
using ProjetoFinal.API.Models;
using ProjetoFinal.API.Repositories;

namespace ProjetoFinal.API.Services;

public sealed class ChamadoService(
    IChamadoRepository repository,
    ICategoriaRepository categoriaRepository) : IChamadoService
{
    public async Task<IReadOnlyList<ChamadoResumoResponse>> ObterTodosAsync(ChamadoFiltroRequest filtro)
    {
        var chamados = await repository.ObterTodosComFiltrosAsync(
            ParaStatusInterno(filtro.Status),
            ParaPrioridadeInterna(filtro.Prioridade),
            ValidarCategoriaId(filtro.CategoriaId));

        return chamados.Select(ParaResumo).ToList();
    }

    public async Task<ChamadoDetalheResponse> ObterPorIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new RequestValidationException("O identificador do chamado deve ser positivo.");
        }

        var chamado = await repository.ObterPorIdComDetalhesAsync(id)
            ?? throw new ResourceNotFoundException("Chamado não encontrado.");

        return ParaDetalhe(chamado);
    }

    public async Task<ChamadoDetalheResponse> AbrirAsync(AbrirChamadoRequest request)
    {
        var titulo = ValidarTexto(request.Titulo, "O título é obrigatório.");
        var descricao = ValidarTexto(request.Descricao, "A descrição é obrigatória.");
        var solicitanteNome = ValidarTexto(request.SolicitanteNome, "O solicitante é obrigatório.");

        if (request.CategoriaId <= 0)
        {
            throw new RequestValidationException("A categoria deve ser informada.");
        }

        if (request.Prioridade is null)
        {
            throw new RequestValidationException("A prioridade deve ser informada.");
        }

        var categoria = await categoriaRepository.ObterPorIdAsync(request.CategoriaId)
            ?? throw new ResourceNotFoundException("Categoria não encontrada.");

        var chamado = new Chamado
        {
            Titulo = titulo,
            Descricao = descricao,
            Prioridade = ParaPrioridadeInterna(request.Prioridade.Value),
            Status = StatusChamado.Aberto,
            SolicitanteNome = solicitanteNome,
            DataAbertura = DateTimeOffset.UtcNow,
            CategoriaId = categoria.Id
        };

        await repository.AdicionarAsync(chamado);
        await repository.SalvarAlteracoesAsync();

        return new ChamadoDetalheResponse(
            chamado.Id,
            chamado.Titulo,
            chamado.Descricao,
            ParaPrioridadePublica(chamado.Prioridade),
            chamado.Status.ToString(),
            chamado.SolicitanteNome,
            chamado.DataAbertura,
            chamado.DataFechamento,
            chamado.Solucao,
            chamado.CategoriaId,
            new CategoriaResponse(categoria.Id, categoria.Nome),
            []);
    }

    public async Task IniciarAsync(int id)
    {
        var chamado = await ObterChamadoParaAtualizacaoAsync(id);
        if (chamado.Status != StatusChamado.Aberto)
        {
            throw new BusinessRuleViolationException("Somente chamados abertos podem iniciar atendimento.");
        }

        chamado.Status = StatusChamado.EmAndamento;
        await repository.SalvarAlteracoesAsync();
    }

    public async Task EncerrarAsync(int id, EncerrarChamadoRequest request)
    {
        var solucao = ValidarTexto(request.Solucao, "A solução é obrigatória.");
        var chamado = await ObterChamadoParaAtualizacaoAsync(id);

        if (chamado.Status != StatusChamado.EmAndamento)
        {
            throw new BusinessRuleViolationException("Somente chamados em andamento podem ser encerrados.");
        }

        chamado.Solucao = solucao;
        chamado.Status = StatusChamado.Fechado;
        chamado.DataFechamento = DateTimeOffset.UtcNow;
        await repository.SalvarAlteracoesAsync();
    }

    public async Task<InteracaoResponse> AdicionarInteracaoAsync(int chamadoId, CriarInteracaoRequest request)
    {
        var autor = ValidarTexto(request.Autor, "O autor é obrigatório.");
        var mensagem = ValidarTexto(request.Mensagem, "A mensagem é obrigatória.");
        var chamado = await ObterChamadoParaAtualizacaoAsync(chamadoId);

        if (chamado.Status == StatusChamado.Fechado)
        {
            throw new BusinessRuleViolationException("Não é possível adicionar interações a um chamado fechado.");
        }

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Autor = autor,
            Mensagem = mensagem,
            DataRegistro = DateTimeOffset.UtcNow
        };

        await repository.AdicionarInteracaoAsync(interacao);
        await repository.SalvarAlteracoesAsync();

        return new InteracaoResponse(
            interacao.Id,
            interacao.ChamadoId,
            interacao.Autor,
            interacao.Mensagem,
            interacao.DataRegistro);
    }

    private async Task<Chamado> ObterChamadoParaAtualizacaoAsync(int id)
    {
        if (id <= 0)
        {
            throw new RequestValidationException("O identificador do chamado deve ser positivo.");
        }

        return await repository.ObterParaAtualizacaoAsync(id)
            ?? throw new ResourceNotFoundException("Chamado não encontrado.");
    }

    private static string ValidarTexto(string valor, string mensagem)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new RequestValidationException(mensagem);
        }

        return valor.Trim();
    }

    private static PrioridadeChamado ParaPrioridadeInterna(PrioridadePublica prioridade) => prioridade switch
    {
        PrioridadePublica.Baixa => PrioridadeChamado.Baixa,
        PrioridadePublica.Media => PrioridadeChamado.Media,
        PrioridadePublica.Alta => PrioridadeChamado.Alta,
        _ => throw new RequestValidationException("A prioridade informada é inválida.")
    };

    private static string ParaPrioridadePublica(PrioridadeChamado prioridade) => prioridade switch
    {
        PrioridadeChamado.Baixa => "Baixa",
        PrioridadeChamado.Media => "Média",
        PrioridadeChamado.Alta => "Alta",
        _ => throw new BusinessRuleViolationException("A prioridade do chamado é inválida.")
    };

    private static StatusChamado? ParaStatusInterno(string? status)
    {
        if (status is null)
        {
            return null;
        }

        return status switch
        {
            "Aberto" => StatusChamado.Aberto,
            "EmAndamento" => StatusChamado.EmAndamento,
            "Fechado" => StatusChamado.Fechado,
            _ => throw new RequestValidationException("O status informado é inválido.")
        };
    }

    private static PrioridadeChamado? ParaPrioridadeInterna(string? prioridade)
    {
        if (prioridade is null)
        {
            return null;
        }

        return prioridade switch
        {
            "Baixa" => PrioridadeChamado.Baixa,
            "Média" => PrioridadeChamado.Media,
            "Alta" => PrioridadeChamado.Alta,
            _ => throw new RequestValidationException("A prioridade informada é inválida.")
        };
    }

    private static int? ValidarCategoriaId(int? categoriaId)
    {
        if (categoriaId is <= 0)
        {
            throw new RequestValidationException("O identificador da categoria deve ser positivo.");
        }

        return categoriaId;
    }

    private static ChamadoResumoResponse ParaResumo(Chamado chamado) =>
        new(
            chamado.Id,
            chamado.Titulo,
            ParaPrioridadePublica(chamado.Prioridade),
            chamado.Status.ToString(),
            chamado.DataAbertura,
            chamado.CategoriaId,
            chamado.Categoria.Nome);

    private static ChamadoDetalheResponse ParaDetalhe(Chamado chamado) =>
        new(
            chamado.Id,
            chamado.Titulo,
            chamado.Descricao,
            ParaPrioridadePublica(chamado.Prioridade),
            chamado.Status.ToString(),
            chamado.SolicitanteNome,
            chamado.DataAbertura,
            chamado.DataFechamento,
            chamado.Solucao,
            chamado.CategoriaId,
            new CategoriaResponse(chamado.Categoria.Id, chamado.Categoria.Nome),
            chamado.Interacoes
                .OrderBy(interacao => interacao.DataRegistro)
                .Select(interacao => new InteracaoResponse(
                    interacao.Id,
                    interacao.ChamadoId,
                    interacao.Autor,
                    interacao.Mensagem,
                    interacao.DataRegistro))
                .ToList());
}
