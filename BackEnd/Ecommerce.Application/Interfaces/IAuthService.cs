using System.Threading.Tasks;
using Ecommerce.Application.DTOs; 

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}