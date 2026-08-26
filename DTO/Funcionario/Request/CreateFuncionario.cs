using UsinaOS.Domain.Enums;

namespace UsinaOS.DTO.Funcionario.Request
{
    public class CreateFuncionario
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public Cargo Cargo { get; set; }
    }
}
