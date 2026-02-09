using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProdutoRepository _repository;

        public ProdutoController(AppDbContext context, IProdutoRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        [HttpGet("vitrine")]
        public async Task<IActionResult> ObterVitrine(
            [FromQuery] string? nome,
            [FromQuery] decimal? precoMin,
            [FromQuery] decimal? precoMax,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 0) 
        {
            var query = _context.Produtos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            if (precoMin.HasValue)
                query = query.Where(p => p.PrecoVenda >= precoMin.Value);

            if (precoMax.HasValue)
                query = query.Where(p => p.PrecoVenda <= precoMax.Value);

            query = query.OrderByDescending(p => p.Id);

            var totalItens = await query.CountAsync();
            List<Produto> itens;
            int totalPaginas = 1;

            if (tamanhoPagina > 0)
            {
                totalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
                itens = await query
                    .Skip((pagina - 1) * tamanhoPagina)
                    .Take(tamanhoPagina)
                    .ToListAsync();
            }
            else
            {
                itens = await query.ToListAsync();
            }

            var resultado = new ResultadoPaginadoDto<Produto>(itens, totalItens, pagina, totalPaginas);

            return Ok(resultado);
        }
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ProdutoCadastroDto dto)
        {
            var existente = await _repository.ObterPorSkuAsync(dto.SKU);
            if (existente != null) return BadRequest("Este SKU já está cadastrado.");

            try
            {
                var produto = new Produto(dto.Nome, dto.SKU, dto.PrecoCusto, dto.PrecoVenda, dto.EstoqueInicial);
                await _repository.AdicionarAsync(produto);
                return CreatedAtAction(nameof(ObterVitrine), new { nome = produto.Nome }, produto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        [HttpPatch("{sku}/estoque")]
        public async Task<IActionResult> AjustarEstoque(string sku, [FromBody] int quantidade)
        {
            var produto = await _repository.ObterPorSkuAsync(sku);
            if (produto == null) return NotFound("Produto não encontrado.");

            try
            {
                produto.AtualizarEstoque(quantidade);
                await _repository.AtualizarAsync(produto);
                return Ok(new { mensagem = "Estoque atualizado com sucesso.", saldoAtual = produto.EstoqueAtual });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("O produto foi alterado por outro usuário. Recarregue a página."); // Tratamento de concorrência otimista (;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}