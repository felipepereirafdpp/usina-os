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


        public async Task<FuncionarioResponse> CadastrarFuncionario(CreateFuncionario dadosFuncionarios)
        {
            if (string.IsNullOrWhiteSpace(dadosFuncionarios.Nome))
            {
                throw new ValidaNomeException("Nome é obrigatorio");
            }
            var cpfCliente = dadosFuncionarios.Cpf;


            if (string.IsNullOrWhiteSpace(cpfCliente))
            {
                throw new ValidaCpfException("CPF é obrigatori");
            }

            bool tamamhoCpf = (cpfCliente.Length == 14);


            if (!tamamhoCpf == false)
            {
                throw new ValidaCpfException("CPF tem que ter mais de 11 digitos");
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



            }catch(DbUpdateException e)
            {
                throw new ArgumentException("Erro ao salvar cadastro");
            }

        }
    }
}
