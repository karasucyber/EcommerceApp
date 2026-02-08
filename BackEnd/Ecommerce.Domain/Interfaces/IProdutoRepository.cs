using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> AdicionarAsync(Produto produto);
        Task<Produto?> ObterPorSkuAsync(string sku);
        Task AtualizarAsync(Produto produto);
    }
}
