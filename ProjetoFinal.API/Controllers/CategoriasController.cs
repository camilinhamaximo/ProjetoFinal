using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Contracts;
using ProjetoFinal.API.DTO.Categoria;
using ProjetoFinal.API.Services;

namespace ProjetoFinal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService) =>
            _categoriaService = categoriaService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> ObterTodas() =>
            Ok(await _categoriaService.ObterTodasAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> ObterPorId(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null) return NotFound(new { message = $"Categoria {id} não encontrada." });
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Criar([FromBody] Categoria categoria)
        {
            var novaCategoria = await _categoriaService.CriarAsync(categoria);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaCategoria.Id }, novaCategoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Categoria categoria)
        {
            await _categoriaService.AtualizarAsync(id, categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _categoriaService.DeletarAsync(id);
            return NoContent();
        }
    }
}