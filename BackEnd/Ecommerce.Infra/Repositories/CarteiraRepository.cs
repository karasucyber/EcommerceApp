using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infra.Repositories
{
    public class CarteiraRepository : ICarteiraRepository
    {
        private readonly AppDbContext _context;

        public CarteiraRepository(AppDbContext context) => _context = context;

        public async Task<Carteira?> ObterPorClienteIdAsync(int clienteId)
        {
            return await _context.Carteiras
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
        }

        public async Task AdicionarAsync(Carteira carteira)
        {
            await _context.Carteiras.AddAsync(carteira);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Carteira carteira)
        {
            _context.Carteiras.Update(carteira);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarMovimentacaoAsync(MovimentacaoCarteira movimentacao)
        {
            await _context.MovimentacoesCarteira.AddAsync(movimentacao);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovimentacaoCarteira>> ObterExtratoAsync(int carteiraId)
        {
            return await _context.MovimentacoesCarteira
                .Where(m => m.CarteiraId == carteiraId)
                .OrderByDescending(m => m.DataCadastro) 
                .ToListAsync();
        }
    }
}