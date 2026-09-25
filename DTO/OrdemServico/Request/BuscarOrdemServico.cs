using UsinaOS.Domain.Enums;

namespace UsinaOS.DTO.OrdemServico.Request
{
    public class BuscarOrdemServico
    {
        public Guid? Id { get; set; }
        public string? NumeroOs { get; set; }
        public string? CpfFuncionario { get; set; }
        public string? RazaoSocial { get; set; }
        public StatusOS? StatusOS { get; set; }
        public Prioridades? PrioridadesOS { get; set; }

    }
}
