using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application;

public class PedidoService : IPedidoService
{
    private readonly EcommerceContext _context;
    public PedidoService(EcommerceContext context) { _context = context; }

    public async Task<Pedido> CriarPedido(int clienteId, List<int> produtosIds)
    {
        var produtos = await _context.Produtos.Where(p => produtosIds.Contains(p.Id)).ToListAsync();
        var pedido = new Pedido { ClienteId = clienteId, Produtos = produtos };
        _context.Pedidos.Add(pedido);
        _context.Historicos.Add(new PedidoHistorico { PedidoId = pedido.Id, StatusAnterior = "N/A", StatusNovo = "Criado" });
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task PagarPedido(int pedidoId)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);
        if (pedido == null) return;
        var statusAnterior = pedido.Status.ToString();
        pedido.Status = PedidoStatus.Pago;
        _context.Historicos.Add(new PedidoHistorico { PedidoId = pedido.Id, StatusAnterior = statusAnterior, StatusNovo = "Pago" });
        await _context.SaveChangesAsync();
    }

    public async Task CancelarPedido(int pedidoId)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);
        if (pedido == null || pedido.Status == PedidoStatus.Pago)
            throw new InvalidOperationException("Nao e possivel cancelar pedido pago");

        var statusAnterior = pedido.Status.ToString();
        pedido.Status = PedidoStatus.Cancelado;
        _context.Historicos.Add(new PedidoHistorico { PedidoId = pedido.Id, StatusAnterior = statusAnterior, StatusNovo = "Cancelado" });
        await _context.SaveChangesAsync();
    }

    public async Task<List<Pedido>> ListarPorStatus(PedidoStatus status)
    {
        return await _context.Pedidos.Where(p => p.Status == status).Include(p => p.Produtos).ToListAsync();
    }
}