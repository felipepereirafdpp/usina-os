using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Peca.Request;
using UsinaOS.DTO.Peca.Response;
using UsinaOS.Exceptions.Peca;
using UsinaOS.Interfaces.Peca;

namespace UsinaOS.Services.Peca
{
    public class PecaService : IPecaService
    {
        private readonly UsinaOSContext _context;

        public PecaService(UsinaOSContext context)
        {
            context = _context;
        }

        private bool ValidaCodigoPeca(string codigoPeca)
        {
            if (codigoPeca == null)
            {
                return false;
            }
            if (codigoPeca.Length != 7)
            {
                return false;
            }
            if (codigoPeca.All(char.IsDigit) == false)
            {
                return false;
            }
            return true;
        }

        public async Task<PecaResponse> CadastrarPeca(CreatePeca dadosPeca)
        {
            if (ValidaCodigoPeca(dadosPeca.CodigoPeca) == false)
            {
                throw new ValidaCodigoPecaException("Codigo Invalido");
            }
            if (string.IsNullOrWhiteSpace(dadosPeca.Nome))
            {
                throw new ValidaNomePecaException("Nome da peça é obrigatoria");
            }
            if (string.IsNullOrWhiteSpace(dadosPeca.DescricaoPeca))
            {
                throw new ValidaDescricaoPeca("Descrição é obrigatoria");
            }
            if (string.IsNullOrWhiteSpace(dadosPeca.MaterialPeca))
            {
                throw new ValidaMaterialPecaException("Material é obrigatorio");
            }

            var pecaNova = new PecaEntitie
            {
                CodigoPeca = dadosPeca.CodigoPeca,
                DescricaoPeca = dadosPeca.DescricaoPeca,
                Nome = dadosPeca.Nome,
                MaterialPeca = dadosPeca.MaterialPeca,
        
            
            };

            try
            {
                _context.Pecas.Add(pecaNova);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao salvar  no banco", e);
            }

            var resposta = new PecaResponse
            {
                Id = pecaNova.Id,
                CodigoPeca = pecaNova.CodigoPeca,
                MaterialPeca = pecaNova.MaterialPeca,
                Nome = pecaNova.Nome,
                
            };

            return resposta;
        }
    }
}
