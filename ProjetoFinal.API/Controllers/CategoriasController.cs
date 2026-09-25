using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Contracts;
using ProjetoFinal.API.DTOs.Categorias;
using ProjetoFinal.API.Services;

namespace ProjetoFinal.API.Controllers;

[ApiController]
[Route("api/categorias")]
[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
public sealed class CategoriasController(ICategoriaService categoriaService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CategoriaResponse>>> ObterTodas() =>
        Ok(await categoriaService.ObterTodasAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaResponse>> ObterPorId(int id) =>
        Ok(await categoriaService.ObterPorIdAsync(id));

    [HttpPost]
    [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoriaResponse>> Criar([FromBody] CriarCategoriaRequest request)
    {
        var novaCategoria = await categoriaService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = novaCategoria.Id }, novaCategoria);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCategoriaRequest request)
    {
        await categoriaService.AtualizarAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Deletar(int id)
    {
        await categoriaService.DeletarAsync(id);
        return NoContent();
    }
}
