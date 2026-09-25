using UsinaOS.DTO.OrdemServico.Response;
using UsinaOS.Interfaces.OrdemServico;

namespace UsinaOS.Services.OrdemServico
{
    public class OrdemServicoService: IOrdemServicoService
    {
        private readonly UsinaOSContext _context;

        public OrdemServicoService(UsinaOSContext context)
        {
            _context = context;
        }

        public async Task<OrdemServicoResponse> CadastrarOs()
        {

        }

    }
}
