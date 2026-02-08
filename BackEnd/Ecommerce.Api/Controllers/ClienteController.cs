using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;

        public ClienteController(IClienteRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteDto dto)
        {
            var existente = await _repository.ObterPorCpfAsync(dto.CPF);
            if (existente != null) return BadRequest("CPF já cadastrado.");

            var cliente = new Cliente(dto.Nome, dto.CPF, dto.Email);
            await _repository.AdicionarAsync(cliente);

            return Ok(new { mensagem = "Cliente cadastrado com sucesso!", id = cliente.Id });
        }

        [HttpGet("buscar/{cpf}")]
        public async Task<IActionResult> BuscarPorCpf(string cpf)
        {
            var cliente = await _repository.ObterPorCpfAsync(cpf);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        [HttpPost("{clienteId}/endereco")]
        public async Task<IActionResult> AdicionarEndereco(int clienteId, [FromBody] EnderecoDto dto)
        {
            var cliente = await _repository.ObterPorIdComEnderecosAsync(clienteId);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            var endereco = new Endereco(dto.Logradouro, dto.Numero, dto.CEP, dto.Cidade);
            cliente.AdicionarEndereco(endereco);


            await _repository.AtualizarAsync(cliente);
            return Ok("Endereço adicionado com sucesso.");
        }
    }
}