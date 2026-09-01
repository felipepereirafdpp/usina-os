using UsinaOS.Domain.Enums;
using UsinaOS.DTO.ItemOrdemServico.Request;

namespace UsinaOS.DTO.OrdemServico.Request
{
    public class UpdateOrdemServico
    {
        public Guid FuncionarioId { get; set; }
        public DateTime DataPrazo { get; set; }
        public Prioridades Prioridade { get; set; }
        public StatusOS Status { get; set; }
        public List<UpdateItemOrdemServico> Itens { get; set; } = new List<UpdateItemOrdemServico>();
    }
}
