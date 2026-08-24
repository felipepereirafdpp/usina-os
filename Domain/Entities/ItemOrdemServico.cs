using System;
using System.ComponentModel.DataAnnotations;

namespace UsinaOS.Domain.Entities;

public class ItemOrdemServico
{
    [Key] public Guid Id { get; set; }

    public Guid OrdemServicoId { get; set; }

    [Required] public OrdemServico OrdemServico { get; set; }

    public Guid PecaId { get; set; }

    [Required] public Peca Peca { get; set; }

    [Required] public int Quantidade { get; set; }


    public ItemOrdemServico() { }

    public ItemOrdemServico(Guid ordemServicoId, Guid pecaId, OrdemServico ordemServico, Peca peca, int quantidade)
    {
        if (ordemServicoId == Guid.Empty)
            throw new ArgumentException("ID da Ordem de Serviço não pode ser vazio.");

        if (ordemServico == null)
            throw new ArgumentException("Ordem de Serviço não pode ser nula.");

        if (pecaId == Guid.Empty)
            throw new ArgumentException("ID da Peça não pode ser vazio.");

        if (peca == null)
            throw new ArgumentException("Peça não pode ser nula.");

        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.");

        OrdemServico = ordemServico;
        OrdemServicoId = ordemServicoId;
        PecaId = pecaId;
        Peca = peca;
        Quantidade = quantidade;
    }
}