using Microsoft.EntityFrameworkCore;
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
                throw new ValidaDescricaoPecaException("Descrição é obrigatoria");
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

        public async Task<PecaResponse> BuscarPecaPorIdPorCodigoPorNome(Guid? id, string? codigoPeca, string? nome)
        {
            if ((id == null || id  == Guid.Empty) && codigoPeca == null && nome == null)
            {
                throw new ValidaBuscaPecaException("O id, código ou nome são obrigatórios");
            }
            

            PecaEntitie? peca = null;

            if (id != null && id != Guid.Empty)
            {
                peca = await _context.Pecas.FindAsync(id);
            }
            else if (codigoPeca != null)
            {
                if (ValidaCodigoPeca(codigoPeca) == true)
                {
                    peca = await _context.Pecas.FirstOrDefaultAsync(u => u.CodigoPeca == codigoPeca);
                    
                }
            }else if(nome != null)
            {
                peca = await _context.Pecas.FirstOrDefaultAsync(u => u.Nome == nome);
            }
            if (peca == null)
            {
                throw new ValidaBuscaPecaException("Parametro invalido");
            }
            var resposta = new PecaResponse{
                Id = peca.Id,
                Nome = peca.Nome,
                CodigoPeca = peca.CodigoPeca,
                MaterialPeca = peca.MaterialPeca,
                
            };
            return resposta;
        }
        public async Task<List<PecaResponse>> ListarPecas()
        {
            var pecas = await _context.Pecas
                .OrderBy(u => u.Nome)
                .Select(peca => new PecaResponse
                {
                    Nome = peca.Nome,
                    CodigoPeca = peca.CodigoPeca,
                    MaterialPeca = peca.MaterialPeca,
                    Id = peca.Id

                }).ToListAsync();

            return (pecas);

        }

        public async Task<PecaResponse> AtualizarPeca(string codigoPeca,UpdatePeca dadosNovosPeca)
        {
            if(ValidaCodigoPeca(codigoPeca) == false)
            {
                throw new ValidaCodigoPecaException("Codigo invalido");
            }
            if (string.IsNullOrWhiteSpace(dadosNovosPeca.Nome))
            {
                throw new ValidaNomePecaException("Nome é obrigatorio");
            }
            if (string.IsNullOrWhiteSpace(dadosNovosPeca.MaterialPeca))
            {
                throw new ValidaMaterialPecaException("Material é obrigatorio");
            }
            if (string.IsNullOrWhiteSpace(dadosNovosPeca.DescricaoPeca))
            {
                throw new ValidaDescricaoPecaException("Descrição é obrigatoria");
            }

            var pecaEncontrado = await _context.Pecas.FirstOrDefaultAsync(u => u.CodigoPeca == codigoPeca);

            if (pecaEncontrado == null)
            {
                throw new ValidaBuscaPecaException("Busca falhou");
            }

            try
            {
                pecaEncontrado.CodigoPeca = dadosNovosPeca.CodigoPeca;
                pecaEncontrado.MaterialPeca = dadosNovosPeca.MaterialPeca;
                pecaEncontrado.Nome = dadosNovosPeca.Nome; 
                pecaEncontrado.DescricaoPeca = dadosNovosPeca.DescricaoPeca;

                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new ArgumentException("Erro ao salvar no banco.", e);
            }
            var resposta = new PecaResponse
            {
                Id = pecaEncontrado.Id,
                CodigoPeca = dadosNovosPeca.CodigoPeca,
                MaterialPeca = dadosNovosPeca.MaterialPeca,
                Nome = dadosNovosPeca.Nome,


            };

            return resposta;
        }

        public Task<bool> DeletarPeca(string codigoPeca)
        {
            if (ValidaCodigoPeca(codigoPeca) == false)
            {
                throw new ValidaCodigoPecaException("Codigo Peca invalida");
            }

           
        }
    }
}
