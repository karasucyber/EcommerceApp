using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RelatorioController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public RelatorioController(IUnitOfWork uow) => _uow = uow;

    [HttpGet("dashboard-financeiro")]
    public async Task<IActionResult> ObterDashboard()
    {
        var pedidosStatusPago = await _uow.Pedidos.ObterTodosAsync();

        var vendasValidas = pedidosStatusPago
            .Where(p => p.Status == StatusPedido.Pago)
            .Select(p => p.ValorTotal)
            .ToList();

        var faturamento = vendasValidas.Sum();
        var totalPedidos = vendasValidas.Count;
        var ticketMedio = totalPedidos > 0 ? faturamento / totalPedidos : 0;


        var todasCarteiras = await _uow.Carteiras.ObterTodosAsync();
        var cashbackExposto = todasCarteiras.Sum(c => c.Saldo);

        var resultado = new DashboardFinanceiroDto(
            faturamento,
            ticketMedio,
            totalPedidos,
            cashbackExposto,
            new List<VendaMensalDto>() 
        );

        return Ok(resultado);
    }

    [HttpGet("curva-abc")]
    public async Task<IActionResult> ObterCurvaAbc()
    {
        var pedidos = await _uow.Pedidos.ObterTodosAsync();
        var pedidosPagos = pedidos.Where(p => p.Status == StatusPedido.Pago).ToList();

        var faturamentoTotal = pedidosPagos.Sum(p => p.ValorTotal);

        var rankingProdutos = pedidosPagos
            .SelectMany(p => p.Itens)
            .GroupBy(i => new { i.ProdutoId, i.NomeProdutoSnapshot })
            .Select(g => new {
                Id = g.Key.ProdutoId,
                Nome = g.Key.NomeProdutoSnapshot,
                Qtd = g.Sum(x => x.Quantidade),
                Total = g.Sum(x => x.Quantidade * x.PrecoUnitario)
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        decimal acumulado = 0;
        var resultado = rankingProdutos.Select(p => {
            acumulado += p.Total;
            var percentualAcumulado = (acumulado / faturamentoTotal) * 100;

            // Atribuição tipada usando o Enum
            ClasseCurvaAbc classe = percentualAcumulado <= 80 ? ClasseCurvaAbc.A :
                                    percentualAcumulado <= 95 ? ClasseCurvaAbc.B :
                                    ClasseCurvaAbc.C;

            return new ItemCurvaAbcDto(
                p.Id,
                p.Nome,
                p.Qtd,
                p.Total,
                (p.Total / faturamentoTotal) * 100,
                classe
            );
        }).ToList();

        return Ok(resultado);
    }
}