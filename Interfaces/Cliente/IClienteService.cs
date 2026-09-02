using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Cliente.Request;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.DTO.Endereco.Request;
using UsinaOS.DTO.Endereco.Response;

namespace UsinaOS.Interfaces.Cliente
{
    public interface IClienteService
    {
        Task<ClienteResponse> CadastrarCliente(CreateCliente InformacoesCliente);
        Task<ClienteResponse> BuscarClientePorIdPorCnpj(Guid? IdCliente, string? cnpjCliente);
        Task<List<ClienteResponse>> ListarClientes();
        Task<ClienteResponse> AtualizarCliente(string cnpjCliente, UpdateCliente InformacoesCliente);
        Task<bool> DeletarCliente(string cnpjCliente);
 
    }
}
