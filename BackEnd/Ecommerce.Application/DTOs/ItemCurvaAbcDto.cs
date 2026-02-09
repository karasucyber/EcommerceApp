using Ecommerce.Domain.Enum;


namespace Ecommerce.Application.DTOs
{
    public record ItemCurvaAbcDto(
        int ProdutoId,
        string Nome,
        int QuantidadeVendida,
        decimal ValorGerado,
        decimal PorcentagemNoFaturamento,
        ClasseCurvaAbc Classe 
    );
}