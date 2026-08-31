
using UsinaOS.Domain.Enums;
using UsinaOS.DTO.ItemOrdemServico.Request;

namespace UsinaOS.DTO.OrdemServico.Request
{
    public class CreateOrdemServico
    {
        public Guid FuncionarioId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataPrazo { get; set; }
        public Prioridades Prioridade { get; set; }
        public List<CreateItemOrdemServico> Itens { get; set; } = new List<CreateItemOrdemServico>();



    }
}
