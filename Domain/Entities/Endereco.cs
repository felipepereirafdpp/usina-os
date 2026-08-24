using System.ComponentModel.DataAnnotations;

namespace UsinaOS.Domain.Entities;

public class Endereco
{
    [Key] public Guid Id { get; set;}
    [Required] [StringLength(9)] public string Cep { get; set; }

    [Required] [StringLength(80)] public string Logradouro { get; set; }

    [Required] [StringLength(5)] public string NumeroPredial { get; set; }

    [Required][StringLength(50)] public string Cidade { get; set; }

    [Required][StringLength(2)] public string Estado { get; set; }

    [Required][StringLength(70)] public string Bairro { get; set; }

    [Required][StringLength(50)] public string Pais { get; set; }



    public Endereco() { }

    public Endereco(string cep, string logradouro, string numeroPredial, string cidade, string estado, string bairro, string pais)

    {

        Cep = cep;
        Logradouro = logradouro;
        NumeroPredial = numeroPredial;
        Cidade = cidade;
        Estado = estado;
        Bairro = bairro;
        Pais = pais;
    }
}