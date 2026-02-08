using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICarteiraRepository
    {
        Task<Carteira?> ObterPorClienteIdAsync(int clienteId);

        Task<IEnumerable<Carteira>> ObterTodosAsync();

        Task AdicionarAsync(Carteira carteira);
        Task AtualizarAsync(Carteira carteira);

        Task AdicionarMovimentacaoAsync(MovimentacaoCarteira movimentacao);

        Task<IEnumerable<MovimentacaoCarteira>> ObterExtratoAsync(int carteiraId);
    }
}