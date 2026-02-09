using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordHasher _hasher;

    public UsuarioController(IUsuarioRepository repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] UsuarioCadastroDto dto)
    {
        // 1. Verifica se o e-mail já existe
        var existente = await _repository.ObterPorEmailAsync(dto.Email);
        if (existente != null) return BadRequest("Este e-mail já está cadastrado.");

        // 2. Criptografa a senha
        var senhaHash = _hasher.Hash(dto.Senha);

        // 3. Cria a entidade usando o seu construtor rico
        var novoUsuario = new Usuario(dto.Nome, dto.Email, senhaHash, (EPerfilUsuario)dto.Perfil);

        // 4. Salva no SQLite
        await _repository.AdicionarAsync(novoUsuario);

        return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] bool ativo)
    {
        // Funcionalidade de Bloqueio/Desbloqueio
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario == null) return NotFound();

        if (ativo) usuario.Ativar(); else usuario.Desativar();

        await _repository.AtualizarAsync(usuario);
        return Ok(new { mensagem = $"Usuário {(ativo ? "ativado" : "bloqueado")} com sucesso." });
    }
}