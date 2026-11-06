using UsinaOS.DTO.Auth.Request;
using UsinaOS.DTO.Auth.Response;

namespace UsinaOS.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginAuthResponse> LoginFuncionario(LoginAuth InformacoesLogin);

        Task<LoginAuthResponse> RefreshToken(RefreshToken Token);
    }
}
