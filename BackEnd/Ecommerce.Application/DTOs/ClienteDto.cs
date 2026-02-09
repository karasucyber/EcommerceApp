using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public record ClienteDto(string Nome, string CPF, string Email);
    public record EnderecoDto(string Logradouro, string Numero, string CEP, string Cidade);
}