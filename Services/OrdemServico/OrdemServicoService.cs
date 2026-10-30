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
    public class OrdemServicoService : IOrdemServicoService
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
            if (dadosOrdemServico.Observacao == null)
            {
                throw new ValidaIdOrdemServicoExceptions("Observação nao pode ser nula)");
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


            int[] registros = new int[dadosOrdemServico.Itens.Count];

            for (int i = 0; i < registros.Length; i++)
            {

                var idpeca = dadosOrdemServico.Itens[i].PecaId;
                var quantidade = dadosOrdemServico.Itens[i].Quantidade;



                if (!await _context.Pecas.AnyAsync(e => e.Id == idpeca))
                {
                    throw new ValidaPecaExcption("A peça não existe");
                }

                if (quantidade <= 0 && quantidade % 1 != 0)

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

            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao salvar", e);
            }

            var resposta = new OrdemServicoResponse
            {
                Id = ordemServico.Id,
                ClienteId = ordemServico.ClienteId,
                Cliente = new ClienteResponse
                {
                    Id = ordemServico.Cliente.Id,
                    Cnpj = ordemServico.Cliente.Cnpj,
                    Endereco = new EnderecoResponse
                    {
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
                FuncionarioResponsavel = new FuncionarioResponse
                {
                    Cargo = ordemServico.Funcionario.Cargo,
                    Email = ordemServico.Funcionario.Email,
                    Nome = ordemServico.Funcionario.Nome,
                    Id = ordemServico.Funcionario.Id
                },
                ItensOrdemServico = ordemServico.ItensOrdemServico.Select(item => new ItemOrdemServicoResponse
                {


                    Peca = new PecaResponse
                    {
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


        public async Task<OrdemServicoResponse> AtualizarOs(int numeroOs, UpdateOrdemServico informacoesOrdemServico)
        {
            // Validações iniciais 
            if (numeroOs == null && numeroOs <= 0)
            {
                throw new ValidaIdOrdemServicoExceptions("Numero da ordem Servico invalido");
            }
            if (informacoesOrdemServico.DataPrazo < DateTime.Today)
            {
                throw new ValidaDataPrazoException("A data prazo é invalida");
            }
            var atualBanco = await _context.OrdemServicos.FirstOrDefaultAsync(u => u.NumeroOrdemServico == numeroOs);

            if (atualBanco == null)
            {
                throw new ArgumentException("Nenhuma OS encontrada");
            }

            if (atualBanco.Status == StatusOS.Concluida || atualBanco.Status == StatusOS.Cancelada)
            {
                throw new ArgumentException("Não é possivel atualizar uma OS concluida ou cancelada");
            }


            if (informacoesOrdemServico?.Prioridade != null)
            {
                if (Enum.IsDefined(typeof(Prioridades), informacoesOrdemServico.Prioridade) == false)
                {
                    throw new ValidaPrioridadeException("A prioridade é invalida");
                }

                if (informacoesOrdemServico.Funcionario.Cargo != Cargo.Encarregado)
                {
                    throw new ValidaPrioridadeException("Somente engarregado pode alterar a prioridade");
                }
            }
            if (informacoesOrdemServico?.Status != null)
            {
                if (Enum.IsDefined(typeof(StatusOS), informacoesOrdemServico.Status) == false)
                {
                    throw new ArgumentException("O status é invalido");
                }

            }

            // crio um novo array com o tamanho dos itens passados 
            int[] registros = new int[informacoesOrdemServico.Itens.Count];

            // pego item po item e vejo seu atributo de pecaId e quantidade de cada item atribuito no array e valido
            for (int i = 0; i < registros.Length; i++)
            {
                var pecaId = informacoesOrdemServico.Itens[i].PecaId;
                var quantidade = informacoesOrdemServico.Itens[i].Quantidade;

                if (!await _context.Pecas.AnyAsync(u => u.Id == pecaId))
                {
                    throw new ValidaListaItensException("Peca não encontrada");
                }

                if (quantidade < 0 && quantidade % 1 != 0)
                {
                    throw new ValidaListaItensException("Quantidade invalida.");
                }


            }
            // vejo se o ite m adicionado ja nao esta na OS que esta sendo editada
            for (int i = 0; i < registros.Length; i++)
            {

                for (int j = 0; j < registros.Length; j++)
                {
                    var ordemServico = await _context.OrdemServicos.Include(o => o.ItensOrdemServico).FirstOrDefaultAsync(u => u.NumeroOrdemServico == numeroOs);

                    var idsPecaBanco = ordemServico.ItensOrdemServico.Select(item => item.Id).ToList();


                    if (informacoesOrdemServico.Itens[i].PecaId == idsPecaBanco[j])
                    {
                        throw new ValidaListaItensException("Essa peça ja esta na lista e não pode ser atualizado ");
                    }

                }

            }

                var itensConvertidos = informacoesOrdemServico.Itens.Select(itemDto =>
                     new ItemOrdemServico
                     {
                         PecaId = itemDto.PecaId,
                         Quantidade = itemDto.Quantidade
                     }).ToList();

                atualBanco.AdicionarItens(itensConvertidos);
            
                atualBanco.FuncionarioId = informacoesOrdemServico.FuncionarioId;
                atualBanco.DataPrazo = informacoesOrdemServico.DataPrazo;
                atualBanco.Observacao = informacoesOrdemServico.Observacao;
                atualBanco.Prioridade = informacoesOrdemServico.Prioridade;
            try
            {
                
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao salvar", e);
            }


            var resposta = new OrdemServicoResponse
            {
                Id = atualBanco.Id,
                ClienteId = atualBanco.ClienteId,
                Cliente = new ClienteResponse
                {
                    Id = atualBanco.Cliente.Id,
                    Cnpj = atualBanco.Cliente.Cnpj,
                    Endereco = new EnderecoResponse
                    {
                        Id = atualBanco.Cliente.Endereco.Id,
                        Cidade = atualBanco.Cliente.Endereco.Cidade,
                        Logradouro = atualBanco.Cliente.Endereco.Logradouro,
                        Pais = atualBanco.Cliente.Endereco.Pais,
                    },
                    Observacao = atualBanco.Cliente.Observacao,
                    RazaoSocial = atualBanco.Cliente.RazaoSocial,
                    Telefone = atualBanco.Cliente.Telefone
                },
                Status = atualBanco.Status,
                Prioridade = atualBanco.Prioridade,
                DataPrazo = atualBanco.DataPrazo,
                DataAbertura = atualBanco.DataAbertura,
                FuncionarioId = atualBanco.FuncionarioId,
                FuncionarioResponsavel = new FuncionarioResponse
                {
                    Cargo = atualBanco.Funcionario.Cargo,
                    Email = atualBanco.Funcionario.Email,
                    Nome = atualBanco.Funcionario.Nome,
                    Id = atualBanco.Funcionario.Id
                },
                ItensOrdemServico = atualBanco.ItensOrdemServico.Select(item => new ItemOrdemServicoResponse
                {


                    Peca = new PecaResponse
                    {
                        CodigoPeca = item.Peca.CodigoPeca,
                        Id = item.Peca.Id,
                        Nome = item.Peca.Nome,
                        DescricaoPeca = item.Peca.DescricaoPeca,
                        MaterialPeca = item.Peca.MaterialPeca
                    },
                    Quantidade = item.Quantidade

                }).ToList(),
                NumeroOrdemServico = atualBanco.NumeroOrdemServico,




            };

            return resposta;

        }
    }
}
