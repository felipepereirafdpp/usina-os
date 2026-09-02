using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UsinaOS.DTO.Cliente.Request;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.Exceptions.Cliente;
using UsinaOS.Interfaces.Cliente;
using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Endereco.Response;
using UsinaOS.Exceptions.Cliente;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.DTO.Endereco.Request;
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
            throw new ArgumentException($"Erro ao conectar com o serviço de validação de endereço: {ex.Message}");
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

    //Lembrar de adicionar o [FromQuery] no controller
    public async Task<ClienteResponse> BuscarClientePorIdPorCnpj(Guid? id, string? cnpj)
    {
        if ((id == null || id == Guid.Empty) && string.IsNullOrWhiteSpace(cnpj))
        {
            throw new ValidaBuscaClienteException("ID ou CNPJ são obrigatorios");
        }
        if (!string.IsNullOrWhiteSpace(cnpj))
        {
            bool validacaoTamanhoCnpj = (cnpj.Length == 14);

            if (validacaoTamanhoCnpj == false)
            {
                throw new ValidaCnpjException("CNPJ inválido. Digite os 14 dígitos correspondentes");
            }
        }

        ClienteEntitie? cliente = null;

        if (id != null)
        {
            cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.Id == id);
        }
        else if (!string.IsNullOrWhiteSpace(cnpj))
        {
            cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.Cnpj == cnpj);
        }
        if (cliente == null)
        {
            throw new ValidaBuscaClienteException("Clinte não encontrado no sistema no banco de dados ! ");
        }
        var resposta = new ClienteResponse
        {
            Id = cliente.Id,
            Cnpj = cliente.Cnpj,
            Endereco = new EnderecoResponse
            {
                Id = cliente.Endereco.Id,
                Cidade = cliente.Endereco.Cidade,
                Logradouro = cliente.Endereco.Logradouro,
                Pais = cliente.Endereco.Pais
            },
            RazaoSocial = cliente.RazaoSocial,
            Telefone = cliente.Telefone,
            Observacao = cliente.Observacao



        };

        return resposta;

    }

    public async Task<List<ClienteResponse>> ListarClientes()
    {
        var clientes = await _context.Clientes.
            OrderBy(u => u.RazaoSocial)
            .Select(cliente => new ClienteResponse
            {
                Id = cliente.Id,
                RazaoSocial = cliente.RazaoSocial,
                Cnpj = cliente.Cnpj,
                Endereco = new EnderecoResponse
                {
                    Id = cliente.Endereco.Id,
                    Cidade = cliente.Endereco.Cidade,
                    Logradouro = cliente.Endereco.Logradouro,
                    Pais = cliente.Endereco.Pais

                },
                Observacao = cliente.Observacao

            }).ToListAsync();

        return (clientes);

    }

    public async Task<ClienteResponse> AtualizarCliente(string cnpjCliente, UpdateCliente informacoesCliente)
    {

        if (string.IsNullOrWhiteSpace(cnpjCliente))
        {
            throw new ValidaCnpjException("CNPJ é obrigatorio");
        }
        if (!string.IsNullOrWhiteSpace(cnpjCliente))
        {
            bool validacaoTamanhoCnpj = (cnpjCliente.Length == 14);

            if (validacaoTamanhoCnpj == false)
            {
                throw new ValidaCnpjException("CNPJ inválido. Digite os 14 dígitos correspondentes");
            }
        }
        if (string.IsNullOrWhiteSpace(informacoesCliente.RazaoSocial))
        {
            throw new ValidaRazaoSocialException("Razão Social é obrigatoria"); 
        }

        if (informacoesCliente == null)
        {
            throw new ValidaBuscaClienteException("Usuario nao informado");
        }

        if (string.IsNullOrWhiteSpace(informacoesCliente.Email))
        {
            throw new ValidaEmailException("Email é obrigatorio");
        }

        var validaEmail = new EmailAddressAttribute().IsValid(informacoesCliente.Email);
        if (!validaEmail)
        {
            throw new ValidaEmailException("Email Invalido");
        }

        bool emailRecebico = await _context.Clientes.AnyAsync(u => u.Email == informacoesCliente.Email && u.Cnpj != cnpjCliente);
        if (emailRecebico)
        {
            throw new ValidaEmailException("Email já cadastrado em outro usuario.");
        }

        if (string.IsNullOrWhiteSpace(informacoesCliente.Telefone))
        {
            throw new FormatException("O Telefone informado é invalido");
        }
        if (!string.IsNullOrWhiteSpace(informacoesCliente.Telefone))
        {

            var telefoneCliente = informacoesCliente.Telefone;
            bool validarTamanhoTelefone = (telefoneCliente.Length == 11);

            if (validarTamanhoTelefone == false)
            {
                throw new FormatException("O Telefone informado é invalido");
            }
        }
        


        var clienteEncontrado = await _context.Clientes.FirstOrDefaultAsync(u => u.Cnpj == cnpjCliente);

        if (clienteEncontrado == null)
        {
            throw new ValidaBuscaClienteException("Usuario nao encontrado");
        }


        if (informacoesCliente.Endereco == null)
        {
            throw new ValidaEnderecoException("O endereco é obrigatorio.");
        }
        if (string.IsNullOrWhiteSpace(informacoesCliente.Endereco.Cep))
        {
            throw new ValidaCepException("Cep Invalido");
        }

        if (!string.IsNullOrWhiteSpace(informacoesCliente.Endereco.Cep))
        {

            var cepDigitado = informacoesCliente.Endereco.Cep;
            bool validarTamanhoCep = (cepDigitado.Length == 8);

            if (validarTamanhoCep == false)
            {
                throw new ValidaCepException();
            }
        }
        try
        {

            string urlAPI = $"https://viacep.com.br/ws/{informacoesCliente.Endereco.Cep}/json/";
            var resultado = await client.GetFromJsonAsync<ViaCepResponse>(urlAPI);

            if (resultado == null || resultado.Erro)
            {
                throw new ValidaCepException("O cep não foi informado nao existe !");

            }
            else
            {
                informacoesCliente.Endereco.Bairro = resultado.Bairro;
                informacoesCliente.Endereco.Cep = resultado.Cep;
                informacoesCliente.Endereco.Logradouro = resultado.Logradouro;
                informacoesCliente.Endereco.Cidade = resultado.Localidade;
                informacoesCliente.Endereco.Estado = resultado.Uf;
                informacoesCliente.Endereco.Pais = "Brasil";
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
        try
        {

            clienteEncontrado.RazaoSocial = informacoesCliente.RazaoSocial;
            clienteEncontrado.Email = informacoesCliente.Email;
            clienteEncontrado.Telefone = informacoesCliente.Telefone;


            clienteEncontrado.Endereco.Cep = informacoesCliente.Endereco.Cep;
            clienteEncontrado.Endereco.Logradouro = informacoesCliente.Endereco.Logradouro;
            clienteEncontrado.Endereco.NumeroPredial = informacoesCliente.Endereco.NumeroPredial;
            clienteEncontrado.Endereco.Cidade = informacoesCliente.Endereco.Cidade;
            clienteEncontrado.Endereco.Estado = informacoesCliente.Endereco.Estado;
            clienteEncontrado.Endereco.Bairro = informacoesCliente.Endereco.Bairro;
            clienteEncontrado.Endereco.Pais = informacoesCliente.Endereco.Pais;

            await _context.SaveChangesAsync();
        }
        


        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar cliente");
        }
        var resposta = new ClienteResponse
        {

            RazaoSocial = informacoesCliente.RazaoSocial,
            Telefone = informacoesCliente.Telefone,
            Endereco = new EnderecoResponse
            {
                Cidade = clienteEncontrado.Endereco.Cidade,
                Logradouro = clienteEncontrado.Endereco.Logradouro,
                Pais = clienteEncontrado.Endereco.Pais,

            },


        };
        return resposta;

    }

    public async Task<bool> DeletarCliente(string cnpjCliente)
    {
        if (string.IsNullOrWhiteSpace(cnpjCliente))
        {
            throw new ValidaCnpjException("CNPJ é obrigatorio");

        }
        if (!string.IsNullOrWhiteSpace(cnpjCliente))
        {
            bool validacaoTamanhoCnpj = (cnpjCliente.Length == 14);

            if (validacaoTamanhoCnpj == false)
            {
                throw new ValidaCnpjException("CNPJ inválido. Digite os 14 dígitos correspondentes");
            }
        }
        var clienteEncontrado = await _context.Clientes.FirstOrDefaultAsync(u => u.Cnpj == cnpjCliente);

        if (clienteEncontrado == null)
        {
            throw new ValidaBuscaClienteException("Cliente não encontrado no sistema");
        }
        try
        {
            _context.Clientes.Remove(clienteEncontrado);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao conectar com o Banco de dados");
            return false;
        }
        return true;
    }
}
