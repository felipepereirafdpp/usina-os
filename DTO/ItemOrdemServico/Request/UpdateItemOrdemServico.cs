
using UsinaOS.DTO.Peca.Response;

namespace UsinaOS.DTO.ItemOrdemServico.Request
{
    public class UpdateItemOrdemServico
    {
        public Guid PecaId { get; set; }
        public PecaResponse Peca { get; set; }
        public int Quantidade { get; set; }
    }
}
