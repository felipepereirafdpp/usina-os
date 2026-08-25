using UsinaOS.DTO.Endereco.Request;

namespace UsinaOS.DTO.Cliente.Request
{
    public class UpdateCliente
    {
        public string RazaoSocial { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public UpdateEndereco Endereco { get; set; }
    }
}
