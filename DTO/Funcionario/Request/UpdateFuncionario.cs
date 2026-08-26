using UsinaOS.Domain.Enums;

namespace UsinaOS.DTO.Funcionario.Request
{
    public class UpdateFuncionario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public Cargo Cargo { get; set; }

    }
}
