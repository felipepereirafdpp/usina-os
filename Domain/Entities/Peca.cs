using System;
using System.ComponentModel.DataAnnotations;

namespace UsinaOS.Domain.Entities;

public class Peca
{
    [Key] public Guid Id { get; set; }

    [Required] public string Nome { get; set; }

    [Required] public string CodigoPeca { get; set; }

    [Required] public string MaterialPeca { get; set; }

    [Required] public string DescricaoPeca { get; set; }


    public string? Observacao { get; set; }

    public Peca() { }

    public Peca(string nome, string codigoPeca, string materialPeca, string descricaoPeca)

    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("Nome da peça não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(codigoPeca))
            throw new ArgumentException("Código da peça não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(materialPeca))
            throw new ArgumentException("Material da peça não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(descricaoPeca))
            throw new ArgumentException("Descrição da peça não pode ser nula ou vazia.");

        Id = Guid.NewGuid();
        Nome = nome;
        CodigoPeca = codigoPeca;
        MaterialPeca = materialPeca;
        DescricaoPeca = descricaoPeca;
    }
}