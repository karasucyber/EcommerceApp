using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application;

public class ProdutoService : IProdutoService
{
    private readonly EcommerceContext _context;
    public ProdutoService(EcommerceContext context) { _context = context; }

    public async Task<Produto> Cadastrar(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<List<Produto>> ListarTodos() => await _context.Produtos.ToListAsync();
}