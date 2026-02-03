using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application;

public class ClienteService : IClienteService
{
    private readonly EcommerceContext _context;
    public ClienteService(EcommerceContext context) { _context = context; }

    public async Task<Cliente> Cadastrar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<List<Cliente>> ListarTodos() => await _context.Clientes.ToListAsync();
}