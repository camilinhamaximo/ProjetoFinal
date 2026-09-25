using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Models;
using ProjetoFinal.API.Services;

namespace ProjetoFinal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChamadosController : ControllerBase
{
    private readonly ChamadoService _chamadoService;

    public ChamadosController(ChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var chamados = await _chamadoService.ObterTodosAsync();
        return Ok(chamados);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var chamado = await _chamadoService.ObterPorIdAsync(id);
        if (chamado == null) return NotFound(new { mensagem = "Chamado não encontrado." });
        return Ok(chamado);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Chamado chamado)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var novoChamado = await _chamadoService.CriarAsync(chamado);
        return CreatedAtAction(nameof(GetById), new { id = novoChamado.Id }, novoChamado);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> PatchStatus(int id, [FromBody] StatusChamado novoStatus)
    {
        var sucesso = await _chamadoService.AtualizarStatusAsync(id, novoStatus);
        if (!sucesso) return NotFound(new { mensagem = "Chamado não encontrado." });
        
        return NoContent();
    }

    [HttpPost("{id}/interacoes")]
    public async Task<IActionResult> PostInteracao(int id, [FromBody] CriarInteracaoDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Mensagem))
            return BadRequest(new { mensagem = "A mensagem da interação não pode ser vazia." });

        var interacao = await _chamadoService.AdicionarInteracaoAsync(id, dto.Mensagem);
        if (interacao == null)
            return BadRequest(new { mensagem = "Não foi possível adicionar a interação. Verifique se o chamado existe." });

        return Ok(interacao);
    }
}

// DTO para tratar a entrada JSON da interação: { "mensagem": "Texto aqui" }
public class CriarInteracaoDto
{
    public string Mensagem { get; set; } = string.Empty;
}