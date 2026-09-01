using System;
using System.ComponentModel.DataAnnotations;

namespace UsinaOS.Domain.Entities;

public class ClienteEntitie
{
    [Key] public Guid Id { get; set; }

    [Required][StringLength(150)] public string RazaoSocial { get; set; }

    [Required][StringLength(14)] public string Cnpj { get; set; }

    [Required][StringLength(15)] public string Telefone { get; set; }

    [Required][StringLength(254)] public string Email { get; set; }

    [StringLength(200)] public string? Observacao { get; set; }

    [Required] public Endereco Endereco { get; set; }


    protected ClienteEntitie() { }

    public ClienteEntitie(string razaoSocial, string telefone, string email, Endereco endereco, string cnpj, string? observacao = null)

    {
        if (string.IsNullOrWhiteSpace(razaoSocial))
            throw new ArgumentException("Razão Social não pode ser nula ou vazia.");

        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("Telefone não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser nulo ou vazio.");

        if (endereco == null)
            throw new ArgumentException("Endereço não pode ser nulo.");

        Id = Guid.NewGuid();
        RazaoSocial = razaoSocial;
        Cnpj = cnpj;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        Observacao = observacao;
    }
}