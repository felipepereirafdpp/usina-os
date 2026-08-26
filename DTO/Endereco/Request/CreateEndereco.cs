namespace UsinaOS.DTO.Endereco.Request
{
    public class CreateEndereco
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string NumeroPredial { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Pais { get; set; }
    }
}
