using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
        string GerarRefreshToken(Usuario usuario); 
    }
}
