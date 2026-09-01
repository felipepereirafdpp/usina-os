using UsinaOS.DTO.Endereco.Request;

namespace UsinaOS.DTO.Cliente.Request
{
    public class CreateCliente
    {
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public CreateEndereco Endereco { get; set; }

        public string? Observacao { get; set; }
    }
}
