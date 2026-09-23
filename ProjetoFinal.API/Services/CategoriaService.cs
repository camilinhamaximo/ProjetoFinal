using Microsoft.EntityFrameworkCore;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Models;

namespace ProjetoFinal.API.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObterTodasAsync();
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<Categoria> CriarAsync(Categoria categoria);
        Task AtualizarAsync(int id, Categoria categoria);
        Task DeletarAsync(int id);
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Categoria>> ObterTodasAsync() =>
            await _context.Categorias.AsNoTracking().ToListAsync();

        public async Task<Categoria?> ObterPorIdAsync(int id) =>
            await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Categoria> CriarAsync(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new ArgumentException("O nome da categoria é obrigatório.");

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task AtualizarAsync(int id, Categoria categoria)
        {
            var categoriaExistente = await _context.Categorias.FindAsync(id);
            if (categoriaExistente == null)
                throw new KeyNotFoundException($"Categoria com ID {id} não foi encontrada.");

            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new ArgumentException("O nome da categoria é obrigatório.");

            categoriaExistente.Nome = categoria.Nome;
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var categoria = await _context.Categorias.Include(c => c.Chamados).FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com ID {id} não foi encontrada.");

            if (categoria.Chamados != null && categoria.Chamados.Any())
                throw new InvalidOperationException("Não é possível excluir uma categoria que possui chamados associados.");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}