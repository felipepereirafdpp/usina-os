using UsinaOS.DTO.Funcionario.Request;
using UsinaOS.DTO.Funcionario.Response;

namespace UsinaOS.Interfaces.Funcionario
{
    public interface IFuncionarioService
    {
        Task<FuncionarioResponse> CadastrarFuncionario(CreateFuncionario informacoesFuncionario);
        Task<FuncionarioResponse> BuscarFuncionarioPorIdPorCpf(Guid? Id,string? CPF);
        Task<List<FuncionarioResponse>> ListarFuncionarios();
        Task<FuncionarioResponse> AtualizarFuncionario(string Cpf,UpdateFuncionario informacoesFuncionario);
        Task<bool> DeletarFuncionario(string CPF);

    }
}
