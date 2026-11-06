using UsinaOS.Domain.Enums;
using UsinaOS.DTO.OrdemServico.Request;
using UsinaOS.DTO.OrdemServico.Response;

namespace UsinaOS.Interfaces.OrdemServico
{
    public interface IOrdemServicoService
    {
        Task<OrdemServicoResponse> CadastrarOs(CreateOrdemServico informacoesOrdemServico);
        
        Task<List<OrdemServicoResponse>> BuscarOsPorFuncionario(BuscarOrdemServico filtrosOrdemServico);
 
        Task<OrdemServicoResponse> AtualizarOs(int numeroOs, UpdateOrdemServico informacoesOrdemServico);
        Task<StatusOrdemServicoResponse> AtualizarStatusOs(int numeroOs,StatusOS statusNovo);
        

    }
}
