using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IUnitOfWork _uow; // porderia ter implementado desde o começo ):

        public PedidoController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
        {
            var pedido = new Pedido(dto.ClienteId, dto.EnderecoId);

            await _uow.BeginTransactionAsync();

            try
            {
                foreach (var itemDto in dto.Itens)
                {
                    var produto = await _uow.Produtos.ObterPorSkuAsync(itemDto.Sku);
                    if (produto == null) return BadRequest($"Produto {itemDto.Sku} não existe.");

                    produto.AtualizarEstoque(-itemDto.Quantidade);
                    await _uow.Produtos.AtualizarAsync(produto);

                    pedido.AdicionarItem(produto.Id, produto.Nome, itemDto.Quantidade, produto.PrecoVenda);
                }

                await _uow.Pedidos.AdicionarAsync(pedido);

                await _uow.CommitTransactionAsync();

                return Ok(new
                {
                    PedidoId = pedido.Id,
                    Total = pedido.ValorTotal,
                    Status = pedido.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                return BadRequest($"Erro ao processar pedido: {ex.Message}");
            }
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var pedido = await _uow.Pedidos.ObterPorIdCompletoAsync(id);
            if (pedido == null) return NotFound("Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Cancelado)
                return BadRequest("Este pedido já está cancelado.");

            await _uow.BeginTransactionAsync();

            try
            {
                foreach (var item in pedido.Itens)
                {
                    var produto = await _uow.Produtos.ObterPorIdAsync(item.ProdutoId);
                    if (produto != null)
                    {
                        produto.AtualizarEstoque(item.Quantidade);
                        await _uow.Produtos.AtualizarAsync(produto);
                    }
                }

                if (pedido.Status == StatusPedido.Pago)
                {
                    var carteira = await _uow.Carteiras.ObterPorClienteIdAsync(pedido.ClienteId);
                    if (carteira != null)
                    {
                        carteira.AdicionarCashback(pedido.ValorTotal);

                        var mov = new MovimentacaoCarteira(
                            carteira.Id,
                            pedido.ValorTotal,
                            TipoMovimentacao.Credito,
                            $"Estorno: Cancelamento Pedido #{pedido.Id}"
                        );

                        await _uow.Carteiras.AdicionarMovimentacaoAsync(mov);
                        await _uow.Carteiras.AtualizarAsync(carteira);
                    }
                }

                pedido.AlterarStatus(StatusPedido.Cancelado);
                await _uow.Pedidos.AtualizarAsync(pedido);

                await _uow.CommitTransactionAsync();

                return Ok(new { mensagem = "Pedido cancelado, estoque e saldo estornados!" });
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                return BadRequest($"Erro no cancelamento: {ex.Message}");
            }
        }
    }
}

