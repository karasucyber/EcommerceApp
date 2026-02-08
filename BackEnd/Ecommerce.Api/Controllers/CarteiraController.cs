using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infra.Context;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CarteiraController : ControllerBase
{
    private readonly ICarteiraRepository _carteiraRepo;
    private readonly IPedidoRepository _pedidoRepo;
    private readonly AppDbContext _context; 

    public CarteiraController(ICarteiraRepository carteiraRepo, IPedidoRepository pedidoRepo, AppDbContext context)
    {
        _carteiraRepo = carteiraRepo;
        _pedidoRepo = pedidoRepo;
        _context = context;
    }

    [HttpPost("creditar")]
    public async Task<IActionResult> Creditar([FromBody] CreditarCashbackDto dto)
    {
        var carteira = await _carteiraRepo.ObterPorClienteIdAsync(dto.ClienteId);

        if (carteira == null)
        {
            carteira = new Carteira(dto.ClienteId);
            await _carteiraRepo.AdicionarAsync(carteira);
        }

        carteira.AdicionarCashback(dto.Valor);
        var mov = new MovimentacaoCarteira(carteira.Id, dto.Valor, TipoMovimentacao.Credito, dto.Motivo);

        await _carteiraRepo.AdicionarMovimentacaoAsync(mov);
        await _carteiraRepo.AtualizarAsync(carteira);

        return Ok(new { SaldoAtual = carteira.Saldo });
    }

    [HttpGet("meu-saldo/{clienteId}")]
    public async Task<IActionResult> ObterSaldoEExtrato(int clienteId)
    {
        // Busca a carteira atualizada
        var carteira = await _carteiraRepo.ObterPorClienteIdAsync(clienteId);

        if (carteira == null)
            return NotFound("Carteira não encontrada para este cliente.");

        // Busca o histórico de movimentações (Compra e Estorno)
        var movimentacoes = await _carteiraRepo.ObterExtratoAsync(carteira.Id);

        var resultado = new SaldoAtualizadoDto(
            carteira.Saldo,
            movimentacoes.Select(m => new MovimentacaoExtratoDto(
                m.Valor,
                m.Tipo.ToString(), // Converte o Enum para Texto (Credito/Debito)
                m.Descricao,
                m.DataCadastro
            )).ToList()
        );

        return Ok(resultado);
    }

    [HttpPost("pagar-pedido/{pedidoId}")]
    public async Task<IActionResult> PagarComCashback(int pedidoId)
    {
        var pedido = await _pedidoRepo.ObterPorIdCompletoAsync(pedidoId);
        if (pedido == null) return NotFound("Pedido inexistente.");
        if (pedido.Status != StatusPedido.Criado) return BadRequest("Pedido não está aguardando pagamento.");

        var carteira = await _carteiraRepo.ObterPorClienteIdAsync(pedido.ClienteId);
        if (carteira == null || carteira.Saldo < pedido.ValorTotal)
            return BadRequest("Saldo de cashback insuficiente.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Debita da Carteira
            carteira.DebitarSaldo(pedido.ValorTotal);
            var mov = new MovimentacaoCarteira(carteira.Id, pedido.ValorTotal, TipoMovimentacao.Debito, $"Pagamento Pedido #{pedido.Id}");
            await _carteiraRepo.AdicionarMovimentacaoAsync(mov);
            await _carteiraRepo.AtualizarAsync(carteira);

            // 2. Muda Status
            pedido.AlterarStatus(StatusPedido.Pago);
            await _pedidoRepo.AtualizarAsync(pedido);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok("Pagamento realizado com sucesso via Cashback!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest("Erro ao processar pagamento: " + ex.Message);
        }


    }
}