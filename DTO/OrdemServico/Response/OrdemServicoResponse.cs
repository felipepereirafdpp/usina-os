using UsinaOS.Domain.Enums;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.DTO.Funcionario.Response;
using UsinaOS.DTO.ItemOrdemServico.Response;

namespace UsinaOS.DTO.OrdemServico.Response
{
    public class OrdemServicoResponse
    {
        public Guid Id { get; set; }
        public string NumeroOrdemServico { get; set; }
        public ClienteResponse Cliente { get; set; }
        public FuncionarioResponse FuncionarioResponsavel { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataPrazo { get; set; }
        public Prioridades Prioridade { get; set; }
        public StatusOS Status { get; set; }
        public List<ItemOrdemServicoResponse> ItensOrdemServico { get; set; } = new List<ItemOrdemServicoResponse>();

    }
}
