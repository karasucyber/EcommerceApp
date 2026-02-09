namespace Ecommerce.Application.DTOs
{
    public record ResultadoPaginadoDto<T>(
        IEnumerable<T> Itens,
        int TotalItens,
        int PaginaAtual,
        int TotalPaginas
    );
}