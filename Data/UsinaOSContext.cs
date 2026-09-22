using Microsoft.EntityFrameworkCore;
using UsinaOS.Domain.Entities;

public class UsinaOSContext(DbContextOptions<UsinaOSContext> options) : DbContext(options)
{
    public DbSet<ClienteEntitie> Clientes { get; set; } = default!;
    public DbSet<Endereco> Enderecos { get; set; } = default!;
    public DbSet<ItemOrdemServico> ItensOredmServico { get; set; } = default!;
    public DbSet<FuncionarioEntitie> Funcionarios { get; set; } = default!;
    public DbSet<PecaEntitie> Pecas { get; set; } = default!;
    public DbSet<OrdemServico> OrdemServicos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Percorre todas as relações e muda o comportamento padrão de Cascade para Restrict
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
