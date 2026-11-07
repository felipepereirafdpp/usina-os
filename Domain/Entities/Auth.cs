using System.ComponentModel.DataAnnotations;
using UsinaOS.DTO.Funcionario.Response;

namespace UsinaOS.Domain.Entities
{
    public class AuthEntitie
    {
        [Key] public Guid Id { get; set; }

        public Guid FuncionarioId { get; set; }
        [Required] public FuncionarioEntitie Funcionario { get; set; }
        [Required][StringLength(20)] public string SenhaHash { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiraEm { get; set; }

    }
}
    