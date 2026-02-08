using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido> AdicionarAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterTodosAsync(); 
        Task<Pedido> AtualizarAsync(Pedido pedido);
        Task<Pedido?> ObterPorIdCompletoAsync(int id);
    }
}
