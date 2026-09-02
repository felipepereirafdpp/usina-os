using UsinaOS.Domain.Enums;
using UsinaOS.DTO.OrdemServico.Request;
using UsinaOS.DTO.OrdemServico.Response;

namespace UsinaOS.Interfaces.OrdemServico
{
    public interface IOrdemServicoService
    {
        Task<OrdemServicoResponse> CadastrarOs(CreateOrdemServico informacoesOrdemServico);
        Task<OrdemServicoResponse> BuscarOsId(Guid id);
        Task<OrdemServicoResponse> BuscarOsNumero(string numeroOs);
        Task<List<OrdemServicoResponse>> BuscarOsPorFuncionario(string CPF);
        Task<List<OrdemServicoResponse>> ListarOsPorCliente(string RazaoSocial);
        Task<List<OrdemServicoResponse>> ListarOsStatus(StatusOS status);
        Task<List<OrdemServicoResponse>> ListarOsPrioridades(Prioridades prioridade);
        Task<OrdemServicoResponse> AtualizarOs(UpdateOrdemServico informacoesOrdemServico);
        Task<OrdemServicoResponse> AtualizarStatusOs(string numeroOs,StatusOS statusNovo);
        Task<bool> DeletarOs(string numeroOs);

    }
}
