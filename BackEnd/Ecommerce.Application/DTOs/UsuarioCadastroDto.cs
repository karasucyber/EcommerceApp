using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Enum;

namespace Ecommerce.Application.DTOs
{
    public record UsuarioCadastroDto(
        string Nome,
        string Email,
        string Senha,
        EPerfilUsuario Perfil 
    );
}