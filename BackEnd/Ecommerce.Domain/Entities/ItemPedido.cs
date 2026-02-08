using System;

namespace Ecommerce.Domain.Entities
{
    public class ItemPedido : EntidadeBase
    {
        public int ProdutoId { get; private set; }
        public int PedidoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; } 

        public string NomeProdutoSnapshot { get; private set; }

        protected ItemPedido() { }

        public ItemPedido(int produtoId, string nomeProduto, int quantidade, decimal precoUnitario)
        {
            ProdutoId = produtoId;
            NomeProdutoSnapshot = nomeProduto; 
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }
}