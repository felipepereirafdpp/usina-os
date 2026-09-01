using UsinaOS.DTO.Peca.Request;
using UsinaOS.DTO.Peca.Response;

namespace UsinaOS.Interfaces.Peca
{
    public interface IPecaService
    {
        Task<PecaResponse> CadastrarPeca(CreatePeca informacoesPeca);
        Task<PecaResponse> BuscarPecaPorId(Guid id);
        Task<PecaResponse> BuscarPecaPorCodigo(string codigoPeca);
        Task<PecaResponse> BuscarPecaPorNome(string nome);
        Task<List<PecaResponse>> ListarPecas();
        Task<bool> DeletarPeca(string codigoPeca);

    }
}
