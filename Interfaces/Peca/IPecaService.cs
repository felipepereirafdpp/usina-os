using UsinaOS.DTO.Peca.Request;
using UsinaOS.DTO.Peca.Response;

namespace UsinaOS.Interfaces.Peca
{
    public interface IPecaService
    {
        Task<PecaResponse> CadastrarPeca(CreatePeca informacoesPeca);
        Task<PecaResponse> BuscarPecaPorIdPorCodigoPorNome(Guid? id, string? codigoPeca, string? nome);
        Task<PecaResponse> AtualizarPeca(string codigoPeca,UpdatePeca informacoesPeca);
        Task<List<PecaResponse>> ListarPecas();
        Task<bool> DeletarPeca(string codigoPeca);

    }
}
