using UsinaOS.Domain.Enums;

namespace UsinaOS.DTO.OrdemServico.Response
{
    public class StatusOrdemServicoResponse
    {
        public string message { get; set; }
        public StatusOS StatusNovo { get; set; }
    }
}
