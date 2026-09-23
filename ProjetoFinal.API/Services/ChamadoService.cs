using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Services
{
    public interface IChamadoService
    {
        Task<IEnumerable<Chamado>> ObterComFiltrosAsync(StatusChamado? status, PrioridadeChamado? prioridade, int? categoriaId);
        Task<Chamado?> ObterPorIdCompletoAsync(int id);
        Task<Chamado> AbrirChamadoAsync(Chamado chamado);
        Task IniciarAtendimentoAsync(int id);
        Task EncerrarChamadoAsync(int id, string solucao);
        Task<Interacao> AdicionarInteracaoAsync(int chamadoId, string autor, string mensagem);
    }

    public class ChamadoService : IChamadoService
    {
        private readonly AppDbContext _context;

        public ChamadoService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Chamado>> ObterComFiltrosAsync(StatusChamado? status, PrioridadeChamado? prioridade, int? categoriaId)
        {
            var query = _context.Chamados.Include(c => c.Categoria).AsQueryable();

            if (status.HasValue) query = query.Where(c => c.Status == status.Value);
            if (prioridade.HasValue) query = query.Where(c => c.Prioridade == prioridade.Value);
            if (categoriaId.HasValue) query = query.Where(c => c.CategoriaId == categoriaId.Value);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Chamado?> ObterPorIdCompletoAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chamado> AbrirChamadoAsync(Chamado chamado)
        {
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == chamado.CategoriaId);
            if (!categoriaExiste)
                throw new ArgumentException($"A categoria informada (ID: {chamado.CategoriaId}) não existe.");

            chamado.Status = StatusChamado.Aberto; // RF06
            chamado.DataAbertura = DateTime.Now;   // RF06
            chamado.DataFechamento = null;
            chamado.Solucao = null;

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();
            return chamado;
        }

        public async Task IniciarAtendimentoAsync(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null)
                throw new KeyNotFoundException($"Chamado com ID {id} não encontrado.");

            if (chamado.Status == StatusChamado.Fechado)
                throw new InvalidOperationException("Não é possível alterar o status de um chamado já encerrado.");

            chamado.Status = StatusChamado.EmAndamento; // RF07
            await _context.SaveChangesAsync();
        }

        public async Task EncerrarChamadoAsync(int id, string solucao)
        {
            if (string.IsNullOrWhiteSpace(solucao))
                throw new ArgumentException("A solução é obrigatória para o encerramento do chamado.");

            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null)
                throw new KeyNotFoundException($"Chamado com ID {id} não encontrado.");

            chamado.Status = StatusChamado.Fechado;  // RF08
            chamado.Solucao = solucao;                // RF08
            chamado.DataFechamento = DateTime.Now;    // RF08

            await _context.SaveChangesAsync();
        }

        public async Task<Interacao> AdicionarInteracaoAsync(int chamadoId, string autor, string mensagem)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                throw new KeyNotFoundException($"Chamado com ID {chamadoId} não encontrado.");

            if (chamado.Status == StatusChamado.Fechado)
                throw new InvalidOperationException("Não é possível registrar interações em chamados encerrados."); // RF10

            if (string.IsNullOrWhiteSpace(autor) || string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("Autor e mensagem da interação devem ser informados.");

            var interacao = new Interacao
            {
                ChamadoId = chamadoId,
                Autor = autor,
                Mensagem = mensagem,
                DataRegistro = DateTime.Now
            };

            _context.Interacoes.Add(interacao);
            await _context.SaveChangesAsync();
            return interacao;
        }
    }
}