using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<MovimentacaoCarteira> MovimentacoesCarteira { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(c => c.CPF).IsUnique();
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasOne<Cliente>()
                    .WithMany(c => c.Enderecos)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Produto>(entity => 
            { entity.HasIndex(p => p.SKU).IsUnique();
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
                entity.Property(p => p.RowVersion).IsConcurrencyToken(); // Controle de concorrência (;

            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.ValorTotal).HasPrecision(18, 2);

                entity.HasMany(p => p.Itens)
                      .WithOne()
                      .HasForeignKey(i => i.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade); // se for apagar os dados para testar ele irá apagar os dados relacionaos
            });

            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.PrecoUnitario).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Carteira>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Saldo).HasPrecision(18, 2);
                entity.HasIndex(c => c.ClienteId).IsUnique();
            });

            modelBuilder.Entity<MovimentacaoCarteira>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Valor).HasPrecision(18, 2);
                // Configura o Enum para ser salvo 
                entity.Property(m => m.Tipo).IsRequired();
            });
        }
    }
}