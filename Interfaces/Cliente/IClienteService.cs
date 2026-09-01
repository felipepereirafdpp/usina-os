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
        Task<ClienteResponse> BuscarClienteID(Guid IdCliente);
        Task<ClienteResponse> BuscarClienteCnpj(string CnpjCliente);
        Task<List<ClienteResponse>> ListarClientes();
        Task<ClienteResponse> AtualizarCliente(UpdateCliente InformacoesCliente);
        Task<bool> DeletarCliente(Guid IdCliente);

 
    }
}
