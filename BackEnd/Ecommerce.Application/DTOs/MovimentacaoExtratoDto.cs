using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public record MovimentacaoExtratoDto(
            decimal Valor,
            string Tipo, // "Credito" ou "Debito"
            string Descricao,
            DateTime Data
        );
}
