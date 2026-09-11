using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using UsinaOS.Domain.Enums;
using UsinaOS.DTO.Funcionario.Request;
using UsinaOS.DTO.Funcionario.Response;
using UsinaOS.Exceptions.Cliente;
using UsinaOS.Exceptions.Funcionario;
using UsinaOS.Interfaces.Funcionario;
using UsinaOS.Domain.Entities;
using BCrypt.Net;

namespace UsinaOS.Services.Funcionario
{
    public class FuncionarioService : IFuncionarioService
    {

        private readonly UsinaOSContext _context;

        public FuncionarioService(UsinaOSContext context)
        {
            _context = context;
        }


        private async Task<bool> ValidaCpf ( string Cpf)
        {
            bool resposta = false;
            var cpfCliente = Cpf;

            if (string.IsNullOrWhiteSpace(cpfCliente))
            {
                resposta = false;
                return resposta;
            }

            bool tamamhoCpf = (cpfCliente.Length == 11);


            if (tamamhoCpf == false)
            {
                resposta = false;
                return resposta;

            }

            int[] vet = cpfCliente.Select(u => (int)Char.GetNumericValue(u)).ToArray();


            var somaPrimeiroNumero = 0;
            var somaTotalPrimeiroNumero = 0;

            var somaSegundoNumero = 0;
            var somaTotalSegundoNumero = 0;

            for (int i = 10, j = 0; j < 9; i--, j++)
            {
                somaPrimeiroNumero = vet[j] * i;
                somaTotalPrimeiroNumero += somaPrimeiroNumero;

            }

            for (int i = 11, j = 0; j < 10; i--, j++)
            {
                somaSegundoNumero = vet[j] * i;

                somaTotalSegundoNumero += somaSegundoNumero;

            }

            var resultadoMultiplicadoPrimeiro = (somaTotalPrimeiroNumero * 10);


            var resultadoMultiplicadoSegundo = (somaTotalSegundoNumero * 10);


            var resultadoRestoSegundo = resultadoMultiplicadoSegundo % 11;
            if (resultadoRestoSegundo == 10)
            {
                resultadoRestoSegundo = 0;
            }
            var resultadoRestoPrimeiro = resultadoMultiplicadoPrimeiro % 11;

            if (resultadoRestoPrimeiro == 10)
            {
                resultadoRestoPrimeiro = 0;
            }

            if (resultadoRestoPrimeiro == vet[9] && resultadoRestoSegundo == vet[10])
            {

                resposta = true;
                return resposta;
            }
            else
            {
                resposta = false;
                return resposta;

            }

            
            
        }


        public async Task<FuncionarioResponse> CadastrarFuncionario(CreateFuncionario dadosFuncionarios)
        {
            if (string.IsNullOrWhiteSpace(dadosFuncionarios.Nome))
            {
                throw new ValidaNomeException("Nome é obrigatorio");
            }
            if(!await ValidaCpf(dadosFuncionarios.Cpf))
            {
                throw new ValidaCpfException("CPF invalido");
            }

            var cpf = await _context.Funcionarios.AnyAsync(u => u.Cpf == dadosFuncionarios.Cpf);
            if (cpf)
            {
                throw new ValidaCpfException("CPF ja cadastrado");
            }

            if (string.IsNullOrWhiteSpace(dadosFuncionarios.Email) || new EmailAddressAttribute().IsValid(dadosFuncionarios.Email))
            {
                throw new ValidaEmailException("Email é obrigatorio");
            }
            if (dadosFuncionarios.Cargo == null)
            {
                throw new ValidaCargoException("Cargo é obrigatorio");
            }
            if (!Enum.IsDefined(typeof(Cargo), dadosFuncionarios.Cargo))
            {
                throw new ValidaCargoException("Cargo não encontrado");
            }
            if (string.IsNullOrWhiteSpace(dadosFuncionarios.SenhaHash))
            {
                throw new ValidaSenhaException("Senha é obrigatoria");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dadosFuncionarios.SenhaHash);

            var funcionarioNovo = new FuncionarioEntitie(
                dadosFuncionarios.Nome,
                dadosFuncionarios.Cpf,
                dadosFuncionarios.Email,
                senhaHash,
                dadosFuncionarios.Cargo

               );

            try
            {
                _context.Funcionarios.Add(funcionarioNovo);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Erro ao salvar cadastro");
            }

            var resposta = new FuncionarioResponse
            {
                Id = funcionarioNovo.Id,
                Email = funcionarioNovo.Email,
                Nome = funcionarioNovo.Nome,
                Cargo = funcionarioNovo.Cargo


            };
            return resposta;

        }

        public async Task<List<FuncionarioResponse>> ListarFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.
                OrderBy(u => u.Nome)
                .Select(funcionarios => new FuncionarioResponse
                {
                    Id = funcionarios.Id,
                    Nome = funcionarios.Nome,
                    Cargo = funcionarios.Cargo,
                    Email = funcionarios.Email


                }).ToListAsync();

            return funcionarios;

        }


        public async Task<FuncionarioResponse> BuscarFuncionarioPorIdPorCpf(Guid? Id, string Cpf)
        {
            if (string.IsNullOrWhiteSpace(Cpf) && (Id.HasValue || Id.Value == Guid.Empty))
            {
                throw new ValidaBuscaCpfId("ID ou Cpf é obrigatorio");
            }
            if (string.IsNullOrWhiteSpace(Cpf))
            {
                throw new ValidaCpfException("CPF é obrigatori");
            }
            if (!string.IsNullOrWhiteSpace(Cpf))
            {
                bool tamamhoCpf = (Cpf.Length == 14);

                if (!tamamhoCpf == false)
                {
                    throw new ValidaCpfException("CPF tem que ter mais de 11 digitos");
                }
            }

            FuncionarioEntitie? funcionario = null;

            if (Id != null)
            {
                funcionario = await _context.Funcionarios.FirstOrDefaultAsync(u => u.Id == Id);
            }
            else if (!string.IsNullOrWhiteSpace(Cpf))
            {
                funcionario = await _context.Funcionarios.FirstOrDefaultAsync(u => u.Cpf == Cpf);

            }
            else
            {
                throw new ValidaBuscaCpfId("Nenhum cliente encontrato");
            }
            var resposta = new FuncionarioResponse
            {
                Cargo = funcionario.Cargo,
                Id = funcionario.Id,
                Email = funcionario.Email,
                Nome = funcionario.Nome
            };

            return resposta;
        }

        public async Task<FuncionarioResponse> AtualizarFuncionario(string cpf, CreateFuncionario dadosFuncionario)
        {
            if (! await ValidaCpf(cpf) == false)
            {
                throw new ValidaCpfException("CPF invalido");
            }


        }
    }
}
