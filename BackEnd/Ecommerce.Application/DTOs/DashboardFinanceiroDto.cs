namespace Ecommerce.Application.DTOs
{
    public record DashboardFinanceiroDto(
        decimal FaturamentoTotal,
        decimal TicketMedio,
        int TotalPedidos,
        decimal CashbackExposto, 
        List<VendaMensalDto> GraficoVendas
    );

    public record VendaMensalDto(string Mes, decimal Total);
}