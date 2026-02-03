using Ecommerce.Domain;
namespace Ecommerce.Application;

public interface IClienteService
{
    Task<Cliente> Cadastrar(Cliente cliente);
    Task<List<Cliente>> ListarTodos();
}