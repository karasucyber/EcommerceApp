using Ecommerce.Domain;
namespace Ecommerce.Application;

public interface IProdutoService
{
    Task<Produto> Cadastrar(Produto produto);
    Task<List<Produto>> ListarTodos();
}