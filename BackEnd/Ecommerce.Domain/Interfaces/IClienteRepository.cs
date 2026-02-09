using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente cliente);
        Task<Cliente?> ObterPorCpfAsync(string cpf); 
        Task<Cliente?> ObterPorIdComEnderecosAsync(int id);
        Task AtualizarAsync(Cliente cliente);
        Task<List<Cliente>> ObterTodosAsync();
    }
}