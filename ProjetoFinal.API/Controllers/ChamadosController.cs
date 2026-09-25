using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Contracts;
using ProjetoFinal.API.DTOs.Chamados;
using ProjetoFinal.API.Services;

namespace ProjetoFinal.API.Controllers;

[ApiController]
[Route("api/chamados")]
[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _chamadoService;

    public ChamadosController(IChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ChamadoResumoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<ChamadoResumoResponse>>> Get([FromQuery] ChamadoFiltroRequest filtro)
    {
        var chamados = await _chamadoService.ObterTodosAsync(filtro);
        return Ok(chamados);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ChamadoDetalheResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChamadoDetalheResponse>> GetById(int id)
    {
        var chamado = await _chamadoService.ObterPorIdAsync(id);
        return Ok(chamado);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChamadoDetalheResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChamadoDetalheResponse>> Post([FromBody] AbrirChamadoRequest request)
    {
        var novoChamado = await _chamadoService.AbrirAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = novoChamado.Id }, novoChamado);
    }

    [HttpPatch("{id:int}/iniciar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Iniciar(int id)
    {
        await _chamadoService.IniciarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/encerrar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Encerrar(int id, [FromBody] EncerrarChamadoRequest request)
    {
        await _chamadoService.EncerrarAsync(id, request);
        return NoContent();
    }

    [HttpPost("{id:int}/interacoes")]
    [ProducesResponseType(typeof(InteracaoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<InteracaoResponse>> PostInteracao(int id, [FromBody] CriarInteracaoRequest request)
    {
        var interacao = await _chamadoService.AdicionarInteracaoAsync(id, request);
        return CreatedAtAction(nameof(GetById), new { id }, interacao);
    }
}
