namespace Ecommerce.Application.DTOs
{
    public record CreditarCashbackDto(int ClienteId, decimal Valor, string Motivo);

    public record ExtratoDto(
        decimal Valor,
        string Tipo, //  Enum para string para o Angular
        string Descricao,
        DateTime Data
    );
}