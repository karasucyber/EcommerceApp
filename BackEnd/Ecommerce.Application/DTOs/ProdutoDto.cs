using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public record ProdutoCadastroDto(
        string Nome,
        string SKU,
        string? Descricao,
        decimal PrecoCusto,
        decimal PrecoVenda,
        int EstoqueInicial
    );

    public record AjusteEstoqueDto(int Quantidade);
}