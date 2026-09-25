using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Services;

public class ChamadoService
{
    private readonly AppDbContext _context;

    public ChamadoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Chamado>> ObterTodosAsync()
    {
        return await _context.Chamados
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes)
            .ToListAsync();
    }

    public async Task<Chamado?> ObterPorIdAsync(int id)
    {
        return await _context.Chamados
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Chamado> CriarAsync(Chamado chamado)
    {
        chamado.DataCriacao = DateTime.UtcNow; // Ajustado de DataAbertura para DataCriacao
        chamado.Status = StatusChamado.Aberto;

        _context.Chamados.Add(chamado);
        await _context.SaveChangesAsync();
        return chamado;
    }

    public async Task<bool> AtualizarStatusAsync(int id, StatusChamado novoStatus)
    {
        var chamado = await _context.Chamados.FindAsync(id);
        if (chamado == null) return false;

        chamado.Status = novoStatus;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Interacao?> AdicionarInteracaoAsync(int chamadoId, string mensagem)
    {
        var chamado = await _context.Chamados.FindAsync(chamadoId);
        if (chamado == null) return null;

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Mensagem = mensagem,
            DataCriacao = DateTime.UtcNow
        };

        _context.Interacoes.Add(interacao);
        await _context.SaveChangesAsync();

        return interacao;
    }
}