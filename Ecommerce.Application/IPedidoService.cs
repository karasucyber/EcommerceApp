using Ecommerce.Domain;
namespace Ecommerce.Application;

public interface IPedidoService
{
    Task<Pedido> CriarPedido(int clienteId, List<int> produtosIds);
    Task PagarPedido(int pedidoId);
    Task CancelarPedido(int pedidoId);
    Task<List<Pedido>> ListarPorStatus(PedidoStatus status);
}