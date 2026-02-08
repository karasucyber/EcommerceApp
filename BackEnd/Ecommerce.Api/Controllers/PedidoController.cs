using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProdutoRepository _produtoRepo;

        public PedidoController(IPedidoRepository pedidoRepo, IProdutoRepository produtoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _produtoRepo = produtoRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
        {
            var pedido = new Pedido(dto.ClienteId, dto.EnderecoId);

            foreach (var itemDto in dto.Itens)
            {
                var produto = await _produtoRepo.ObterPorSkuAsync(itemDto.Sku);
                if (produto == null) return BadRequest($"Produto {itemDto.Sku} não existe.");

                try
                {
                    produto.AtualizarEstoque(-itemDto.Quantidade); // Baixa estoque
                    await _produtoRepo.AtualizarAsync(produto);

                    pedido.AdicionarItem(produto.Id, itemDto.Quantidade, produto.PrecoVenda);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro no produto {produto.Nome}: {ex.Message}");
                }
            }

            await _pedidoRepo.AdicionarAsync(pedido);
            return Ok(new { PedidoId = pedido.Id, Total = pedido.ValorTotal, Status = "Criado" });
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var pedido = await _pedidoRepo.ObterPorIdCompletoAsync(id);

            if (pedido == null) return NotFound("Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Cancelado)
                return BadRequest("Este pedido já está cancelado.");

            try
            {
                pedido.AlterarStatus(StatusPedido.Cancelado);

                foreach (var item in pedido.Itens)
                {
                    var produto = await _produtoRepo.ObterPorIdAsync(item.ProdutoId);

                    if (produto != null)
                    {
                        produto.AtualizarEstoque(item.Quantidade);
                        await _produtoRepo.AtualizarAsync(produto);
                    }
                }

                await _pedidoRepo.AtualizarAsync(pedido);

                return Ok(new { mensagem = "Pedido cancelado e estoque estornado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}