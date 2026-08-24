using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UsinaOS.Domain.Enums;

namespace UsinaOS.Domain.Entities;

public class OrdemServico
{
    [Key] public Guid Id { get; set; }


    [Required][StringLength(50)] public string NumeroOrdemServico { get; set; }



    public Guid ClienteId { get; set; }


    [Required] public Cliente Cliente { get; set; }


    [Required] public DateTime DataAbertura { get; set; }


    [Required] public DateTime DataPrazo { get; set; }


    [Required] public Prioridades Prioridade { get; set; }


    [Required] public StatusOS Status { get; set; }

    public string? Observacao { get; set; }

    private readonly List<ItemOrdemServico> _itensOrdemServico = new();

    public IReadOnlyCollection<ItemOrdemServico> ItensOrdemServico
        => _itensOrdemServico;

    public OrdemServico() { }

    public OrdemServico(string numeroOrdemServico, Cliente cliente, DateTime dataAbertura, DateTime dataPrazo, Prioridades prioridades, StatusOS status, string? observacao = null)

    {
        if (string.IsNullOrWhiteSpace(numeroOrdemServico))
            throw new ArgumentException("Número da Ordem de Serviço não pode ser nulo ou vazio.");

        if (cliente == null)
            throw new ArgumentException("Cliente não pode ser nulo.");

        if (dataAbertura == default)
            throw new ArgumentException("Data de Abertura não pode ser nula.");

        if (dataPrazo == default)
            throw new ArgumentException("Data de Prazo não pode ser nula.");

        if (dataPrazo < dataAbertura)
            throw new ArgumentException("Data do prazo não pode ser menor que a Data de Abertura.");

        Id = Guid.NewGuid();
        NumeroOrdemServico = numeroOrdemServico;
        ClienteId = cliente.Id;
        Cliente = cliente;
        DataAbertura = dataAbertura;
        DataPrazo = dataPrazo;
        Prioridade = prioridades;
        Status = status;
        Observacao = observacao;
    }
}