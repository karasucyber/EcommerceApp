namespace Ecommerce.Domain.Entities
{
    public class Produto : EntidadeBase
    {
        public string Nome { get; private set; }
        public string SKU { get; private set; } 
        public string? Descricao { get; private set; }
        public decimal PrecoCusto { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public int EstoqueAtual { get; private set; }

        public string RowVersion { get; private set; } = Guid.NewGuid().ToString();

        protected Produto() { }

        public Produto(string nome, string sku, decimal custo, decimal venda, int estoqueInicial)
        {
            if (venda <= custo)
                throw new ArgumentException("O preço de venda deve ser maior que o custos.");

            Nome = nome;
            SKU = sku;
            PrecoCusto = custo;
            PrecoVenda = venda;
            EstoqueAtual = estoqueInicial;
        }

        public void AtualizarEstoque(int quantidade)
        {
            if (EstoqueAtual + quantidade < 0)
                throw new InvalidOperationException("Estoque insuficiente para esta operação.");

            EstoqueAtual += quantidade;
            RowVersion = Guid.NewGuid().ToString();
            AtualizarData();
        }
    }
}