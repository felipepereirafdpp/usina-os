using System;
using System.ComponentModel.DataAnnotations;
using UsinaOS.Domain.Enums;

namespace UsinaOS.Domain.Entities;

public class Funcionario
{
    [Key] public Guid Id { get; set; }

    [Required] [StringLength(150)] public string Nome { get; set; }

    [Required] [StringLength(14)] public string Cpf { get; set; }

    [Required] [StringLength(254)] public string Email { get; set; }

    [Required] public string SenhaHash { get; set; }

    [Required] public Cargo Cargo { get; set; }


    protected Funcionario() { }

    public Funcionario(string nome, string cpf, string email, string senhaHash, Cargo cargo)

    {
        if (string.IsNullOrWhiteSpace(nome)) { 
            throw new ArgumentException("Nome não pode ser nulo ou vazio.");
        }

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Senha não pode ser nula ou vazia.");

        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Email = email;
        SenhaHash = senhaHash;
        Cargo = cargo;
    }
}