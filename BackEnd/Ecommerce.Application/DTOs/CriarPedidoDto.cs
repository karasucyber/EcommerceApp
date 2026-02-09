using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public record CriarPedidoDto(
           int ClienteId,
           int EnderecoId,
           List<ItemPedidoDto> Itens
       );
}
