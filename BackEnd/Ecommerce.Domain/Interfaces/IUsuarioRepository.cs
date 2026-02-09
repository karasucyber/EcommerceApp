using Ecommerce.Domain.Entities;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email);

        Task<Usuario?> ObterPorIdAsync(int id);

        Task<Usuario?> ObterPorRefreshTokenAsync(string refreshToken);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
    }
}