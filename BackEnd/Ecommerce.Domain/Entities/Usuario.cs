using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set;}
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public EPerfilUsuario perfil { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiry { get; private set; }
        protected Usuario(){}

        public Usuario(string nome, string email, string senha, EPerfilUsuario perfilUsuario)
        {
            if (string.IsNullOrWhiteSpace(nome)) { throw new ArgumentException("O nome é obrigatorio."); }
            if (string.IsNullOrWhiteSpace(email)) { throw new ArgumentException("O email é obrigatorio"); }
            if (string.IsNullOrWhiteSpace(senha)) { throw new ArgumentException("A senha é obrigatória"); }

            Nome = nome;
            Email = email.ToLower().Trim();
            Senha = senha;
            perfil = perfilUsuario; 
        }

        public void AlterarSenha(string novasenha)
        {
            if (string.IsNullOrWhiteSpace(novasenha)) {throw new ArgumentException("A nova senha é obrigatória");}
            Senha = novasenha;
            AtualizarData();
        }
        public void AtualizarRefreshToken(string token, DateTime expiry)
        {
            RefreshToken = token;
            RefreshTokenExpiry = expiry;
        }
    }
}
