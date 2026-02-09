using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(
                            IUsuarioRepository usuarioRepository,
                            ITokenService tokenService,
                            IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario == null) return null;
            if (!_passwordHasher.Verify(request.Senha, usuario.Senha)) return null;
            if (!usuario.Ativo) throw new Exception("Esta conta foi desativa entre em contato com o ADM");

            var accessToken = _tokenService.GerarToken(usuario);
            var refreshToken = _tokenService.GerarRefreshToken(usuario);

            usuario.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

            await _usuarioRepository.AtualizarAsync(usuario);

            return new LoginResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.perfil.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiracao = DateTime.UtcNow.AddHours(2) 
            };
        }
    }
}
