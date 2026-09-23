using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Models;
using ProjetoFinal.API.Services;

namespace ProjetoFinal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly IChamadoService _chamadoService;

        public ChamadosController(IChamadoService chamadoService) =>
            _chamadoService = chamadoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chamado>>> ObterComFiltros(
            [FromQuery] StatusChamado? status,
            [FromQuery] PrioridadeChamado? prioridade,
            [FromQuery] int? categoriaId)
        {
            return Ok(await _chamadoService.ObterComFiltrosAsync(status, prioridade, categoriaId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chamado>> ObterPorId(int id)
        {
            var chamado = await _chamadoService.ObterPorIdCompletoAsync(id);
            if (chamado == null) return NotFound(new { message = $"Chamado {id} não encontrado." });
            return Ok(chamado);
        }

        [HttpPost]
        public async Task<ActionResult<Chamado>> AbrirChamado([FromBody] Chamado chamado)
        {
            var novoChamado = await _chamadoService.AbrirChamadoAsync(chamado);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoChamado.Id }, novoChamado);
        }

        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> IniciarAtendimento(int id)
        {
            await _chamadoService.IniciarAtendimentoAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/encerrar")]
        public async Task<IActionResult> EncerrarChamado(int id, [FromBody] EncerrarChamadoDto dto)
        {
            await _chamadoService.EncerrarChamadoAsync(id, dto.Solucao);
            return NoContent();
        }

        [HttpPost("{id}/interacoes")]
        public async Task<ActionResult<Interacao>> AdicionarInteracao(int id, [FromBody] InteracaoDto dto)
        {
            var interacao = await _chamadoService.AdicionarInteracaoAsync(id, dto.Autor, dto.Mensagem);
            return CreatedAtAction(nameof(ObterPorId), new { id }, interacao);
        }
    }

    public class EncerrarChamadoDto
    {
        public string Solucao { get; set; } = string.Empty;
    }

    public class InteracaoDto
    {
        public string Autor { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}