namespace Ecommerce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedidoRepository Pedidos { get; }
        IProdutoRepository Produtos { get; }
        ICarteiraRepository Carteiras { get; }

        Task<bool> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}