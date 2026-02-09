using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Enum
{
public enum StatusPedido
    {
        Criado = 1,
        AguardandoPagamento = 2,
        Pago = 3,
        Enviado = 4,
        Entregue = 5,
        Cancelado = 6
    }
}
