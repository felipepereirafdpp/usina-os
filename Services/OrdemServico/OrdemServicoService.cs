using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UsinaOS.Domain.Entities;
using UsinaOS.Domain.Enums;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.DTO.Endereco.Response;
using UsinaOS.DTO.Funcionario.Response;
using UsinaOS.DTO.ItemOrdemServico.Response;
using UsinaOS.DTO.OrdemServico.Request;
using UsinaOS.DTO.OrdemServico.Response;
using UsinaOS.DTO.Peca.Response;
using UsinaOS.Exceptions.OrdemServico;
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

        public async Task<OrdemServicoResponse> CadastrarOs(CreateOrdemServico dadosOrdemServico)
        {
            if (dadosOrdemServico.ClienteId == Guid.Empty)
            {
                throw new ValidaIdOrdemServicoExceptions("ID do Cliente Invalido");

            }
            if (dadosOrdemServico.FuncionarioId == Guid.Empty)
            {
                throw new ValidaIdOrdemServicoExceptions("ID do Funcionario Invalido");
            }
            if (dadosOrdemServico.DataPrazo < DateTime.Today)
            {
                throw new ValidaDataPrazoException("A data prazo é invalida");
            }
            if (dadosOrdemServico?.Prioridade == null)
            {
                throw new ValidaPrioridadeException("Prioridade Invalida"); 
            }
            if (dadosOrdemServico.Itens == null || dadosOrdemServico.Itens.Count == 0)
            {
                throw new ValidaListaItensException("Lista esta vazia ");
            }


            int[] registros = new int [dadosOrdemServico.Itens.Count];

            for (int i = 0; i < registros.Length; i++) {

                var idpeca = dadosOrdemServico.Itens[i].PecaId;
                var quantidade = dadosOrdemServico.Itens[i].Quantidade;

                

                if (!await _context.Pecas.AnyAsync(e => e.Id == idpeca))
                {
                    throw new ValidaPecaExcption("A peça não existe");
                }
              
                if (quantidade <= 0 || quantidade % 1 != 0)
                
                {
                    throw new ValidaPecaExcption("A peça tem que ser maior que 0");
                }

            }
            for (int i = 0; i < registros.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (dadosOrdemServico.Itens[i].PecaId == dadosOrdemServico.Itens[j].PecaId)
                    {
                        throw new ValidaPecaExcption("A peça ja esta na OS");
                    }
                }
            }
            var itensConvertidos = dadosOrdemServico.Itens.Select(itemDto => new ItemOrdemServico
            {
                
                PecaId = itemDto.PecaId,
                Quantidade = itemDto.Quantidade,
              
               
            });

            var ordemServico = new OrdemServicoEntitie
            {
                FuncionarioId = dadosOrdemServico.FuncionarioId,
                ClienteId = dadosOrdemServico.ClienteId,
                DataAbertura = DateTime.Now,
                DataPrazo = dadosOrdemServico.DataPrazo,
                Observacao = dadosOrdemServico.Observacao,
                Prioridade = dadosOrdemServico.Prioridade,

            };
            ordemServico.AdicionarItens(itensConvertidos);

            try
            {
                _context.OrdemServicos.Add(ordemServico);
                await _context.SaveChangesAsync();

            }catch(Exception e)
            {
                throw new ArgumentException("Erro ao salvar", e);
            }

            var resposta = new OrdemServicoResponse
            {
                Id = ordemServico.Id,
                ClienteId = ordemServico.ClienteId,
                Cliente = new ClienteResponse {
                    Id = ordemServico.Cliente.Id,
                    Cnpj = ordemServico.Cliente.Cnpj,
                    Endereco = new EnderecoResponse {
                        Id = ordemServico.Cliente.Endereco.Id,
                        Cidade = ordemServico.Cliente.Endereco.Cidade,
                        Logradouro = ordemServico.Cliente.Endereco.Logradouro,
                        Pais = ordemServico.Cliente.Endereco.Pais,
                    },
                    Observacao = ordemServico.Cliente.Observacao,
                    RazaoSocial = ordemServico.Cliente.RazaoSocial,
                    Telefone = ordemServico.Cliente.Telefone
                },
                Status = StatusOS.Aberta,
                Prioridade = ordemServico.Prioridade,
                DataPrazo = dadosOrdemServico.DataPrazo,
                DataAbertura = ordemServico.DataAbertura,
                FuncionarioId = ordemServico.FuncionarioId,
                FuncionarioResponsavel = new FuncionarioResponse { 
                    Cargo = ordemServico.Funcionario.Cargo,
                    Email = ordemServico.Funcionario.Email,
                    Nome = ordemServico.Funcionario.Nome,
                    Id = ordemServico.Funcionario.Id
                },
                ItensOrdemServico = ordemServico.ItensOrdemServico.Select(item => new ItemOrdemServicoResponse
                {
                
                
                    Peca = new PecaResponse { 
                        CodigoPeca = item.Peca.CodigoPeca,
                        Id = item.Peca.Id,
                        Nome = item.Peca.Nome,
                        DescricaoPeca = item.Peca.DescricaoPeca,
                        MaterialPeca = item.Peca.MaterialPeca
                    },
                    Quantidade = item.Quantidade
                  
                }).ToList(),
                NumeroOrdemServico = ordemServico.NumeroOrdemServico,


            };

            return resposta;
        }


        public async Task<OrdemServicoResponse> AtualizarOs(UpdateOrdemServico informacoesOrdemServico)
        {
            
        }
    }
}
