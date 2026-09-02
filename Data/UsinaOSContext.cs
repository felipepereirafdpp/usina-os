using Microsoft.EntityFrameworkCore;
using UsinaOS.Domain.Entities;

public class UsinaOSContext(DbContextOptions<UsinaOSContext> options) : DbContext(options)
{
    public DbSet<ClienteEntitie> Clientes { get; set; } = default!;
    public DbSet<Endereco> Enderecos { get; set; } = default!;
    public DbSet<ItemOrdemServico> ItensOredmServico { get; set; } = default!;
    public DbSet<Funcionario> Funcionarios { get; set; } = default!;
    public DbSet<Peca> Pecas { get; set; } = default!;
    public DbSet<OrdemServico> OrdemServicos { get; set; } = default!;
}
