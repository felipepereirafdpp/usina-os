using UsinaOS.DTO.Endereco.Response;

namespace UsinaOS.DTO.Cliente.Response
{
    public class ClienteResponse
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }

        public EnderecoResponse Endereco { get; set; }

        public string? Observacao { get; set; }
    }
}
