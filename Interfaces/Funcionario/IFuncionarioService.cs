using UsinaOS.DTO.Funcionario.Request;
using UsinaOS.DTO.Funcionario.Response;

namespace UsinaOS.Interfaces.Funcionario
{
    public interface IFuncionarioService
    {
        Task<FuncionarioResponse> CadastrarFuncionario(CreateFuncionario informacoesFuncionario);
        Task<FuncionarioResponse> BuscarFuncionarioId(Guid Id);
        Task<FuncionarioResponse> BuscarFuncionarioCPF(string CPF);
        Task<List<FuncionarioResponse>> ListarFuncionarios();
        Task<FuncionarioResponse> AtualizarFuncionario(UpdateFuncionario informacoesFuncionario);
        Task<bool> DeletarFuncionario(string CPF);

    }
}
