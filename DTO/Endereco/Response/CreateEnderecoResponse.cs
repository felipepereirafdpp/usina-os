namespace UsinaOS.DTO.Endereco.Response
{
    public class CreateEnderecoResponse
    {
        public Guid Id { get; set; }
        public string Logradouro { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
    }
}
