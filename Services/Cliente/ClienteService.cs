using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UsinaOS.DTO.Cliente.Request;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.Exceptions.Cliente;
using UsinaOS.Interfaces.Cliente;
using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Endereco.Response;
namespace UsinaOS.Services.Cliente;

public class ClienteService : IClienteService
{
    public class ViaCepResponse
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public bool Erro { get; set; }
    }
    private static readonly HttpClient client = new HttpClient();
    private readonly UsinaOSContext _context;
    public ClienteService(UsinaOSContext context)
    {
        _context = context;
    }

    public async Task<ClienteResponse> CadastrarCliente(CreateCliente InformacoesCliente)
    {
        if (string.IsNullOrWhiteSpace(InformacoesCliente.RazaoSocial))
        {
            throw new ValidaRazaoSocialException("A razão social é obrigatorio !");
        }


        if (string.IsNullOrWhiteSpace(InformacoesCliente.Cnpj))
        {
            throw new ValidaCnpjException("O CNPJ é obrigatorio !");
        }

        var cnpjCliente = InformacoesCliente.Cnpj;
        bool validacaoTamanhoCnpj = (cnpjCliente.Length == 14);

        if (validacaoTamanhoCnpj == false)
        {
            throw new ValidaCnpjException("CNPJ inválido. Digite os 14 dígitos correspondentes");
        }


        var CNPJ = await _context.Clientes.AnyAsync(u => u.Cnpj == InformacoesCliente.Cnpj);
        if (CNPJ)
        {
            throw new ValidaCnpjException("O CNPJ já esta cadastrado");
        }

        var validarArrobaEmail = new EmailAddressAttribute().IsValid(InformacoesCliente.Email);
        if (!validarArrobaEmail)
        {
            throw new FormatException("O formato do e-mail é inválido.");
        }

        var validarEmailCadastrado = await _context.Clientes.AnyAsync(u => u.Email == InformacoesCliente.Email);
        if (validarEmailCadastrado)
        {
            throw new ValidaEmailException("O email informado já foi cadastrado.");
        }

        var telefoneCliente = InformacoesCliente.Telefone;
        bool validarTamanhoTelefone = (telefoneCliente.Length == 11);
        if (validarTamanhoTelefone == false)
        {
            throw new FormatException("O Telefone informado é invalido");
        }

        var cepDigitado = InformacoesCliente.Endereco.Cep;
        bool validarTamanhoCep = (cepDigitado.Length == 8);

        if (validarTamanhoCep == false)
        {
            throw new ValidaCepException();
        }

        try
        {

            string urlAPI = $"https://viacep.com.br/ws/{cepDigitado}/json/";
            var resultado = await client.GetFromJsonAsync<ViaCepResponse>(urlAPI);

            if (resultado == null || resultado.Erro)
            {
                throw new ValidaCepException("O cep não foi informado nao existe !");

            }
            else
            {
                InformacoesCliente.Endereco.Bairro = resultado.Bairro;
                InformacoesCliente.Endereco.Cep = resultado.Cep;
                InformacoesCliente.Endereco.Logradouro = resultado.Logradouro;
                InformacoesCliente.Endereco.Cidade = resultado.Localidade;
                InformacoesCliente.Endereco.Estado = resultado.Uf;
                InformacoesCliente.Endereco.Pais = "Brasil";
            }
        }
        catch (ValidaCepException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao conectar com o serviço de validação de endereço: {ex.Message}");
        }

        var cliente = new ClienteEntitie(

            InformacoesCliente.RazaoSocial,
            InformacoesCliente.Telefone,
            InformacoesCliente.Email,
              new Endereco
              (
                   InformacoesCliente.Endereco.Cep,
                   InformacoesCliente.Endereco.NumeroPredial,
                  InformacoesCliente.Endereco.Logradouro,
                   InformacoesCliente.Endereco.Bairro,
                   InformacoesCliente.Endereco.Cidade,
                   InformacoesCliente.Endereco.Estado,
                   InformacoesCliente.Endereco.Pais
              ),
            InformacoesCliente.Cnpj,
            InformacoesCliente.Observacao


        );
        try
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Erro ao salvar o cliente");
        }

        var resposta = new ClienteResponse
        {
            Id = cliente.Id,
            RazaoSocial = cliente.RazaoSocial,
            Cnpj = cliente.Cnpj,
            Telefone = cliente.Telefone,
            Endereco = new EnderecoResponse
            {
                Id = cliente.Endereco.Id,
                Cidade = cliente.Endereco.Cidade,
                Logradouro = cliente.Endereco.Logradouro,
                Pais = cliente.Endereco.Pais


            },
            Observacao = cliente.Observacao

        };
        return resposta;
    }



}
