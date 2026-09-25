using ProjetoFinal.API.DTOs.Chamados;
namespace ProjetoFinal.API.Services;

public interface IChamadoService
{
    Task<IReadOnlyList<ChamadoResumoResponse>> ObterTodosAsync(ChamadoFiltroRequest filtro);
    Task<ChamadoDetalheResponse> ObterPorIdAsync(int id);
    Task<ChamadoDetalheResponse> AbrirAsync(AbrirChamadoRequest request);
    Task IniciarAsync(int id);
    Task EncerrarAsync(int id, EncerrarChamadoRequest request);
    Task<InteracaoResponse> AdicionarInteracaoAsync(int chamadoId, CriarInteracaoRequest request);
}
