using UsinaOS.DTO.Funcionario.Response;

namespace UsinaOS.Domain.Entities
{
    public class AuthEntitie
    {
        public Guid Id { get; set; }

        public Guid FuncionarioId { get; set; }
        public FuncionarioEntitie Funcionario { get; set; }
        public string SenhaHash { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiraEm { get; set; }

    }
}
    