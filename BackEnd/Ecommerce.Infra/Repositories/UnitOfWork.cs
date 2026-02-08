using Ecommerce.Domain.Interfaces;
using Ecommerce.Infra.Context;

namespace Ecommerce.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IPedidoRepository Pedidos { get; private set; }
        public IProdutoRepository Produtos { get; private set; }
        public ICarteiraRepository Carteiras { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Pedidos = new PedidoRepository(_context);
            Produtos = new ProdutoRepository(_context);
            Carteiras = new CarteiraRepository(_context);
        }

        public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;
        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitTransactionAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackTransactionAsync() => await _context.Database.RollbackTransactionAsync();

        public void Dispose() => _context.Dispose();
    }
}