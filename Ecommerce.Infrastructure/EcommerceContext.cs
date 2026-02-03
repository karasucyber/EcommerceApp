using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure;

public class EcommerceContext : DbContext
{
    public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<Pedido> Pedidos { get; set; } = null!;
    public DbSet<PedidoHistorico> Historicos { get; set; } = null!;
}