using UsinaOS.Domain.Enums;

namespace UsinaOS.DTO.Funcionario.Response
{
    public class FuncionarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Cargo Cargo { get; set; }
    }
}
