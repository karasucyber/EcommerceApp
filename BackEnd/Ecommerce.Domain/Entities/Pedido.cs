using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Pedido : EntidadeBase
    {
        public int ClienteId { get; private set; }
        public int EnderecoId { get; private set; } // Id endereco criado pelo usu
        public decimal ValorTotal { get; private set; }
        public StatusPedido Status { get; private set; }

        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens;

        protected Pedido() { }

        public Pedido(int clienteId, int enderecoId)
        {
            ClienteId = clienteId;
            EnderecoId = enderecoId;
            Status = StatusPedido.Criado;
            ValorTotal = 0;
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _itens.Sum(item => item.PrecoUnitario * item.Quantidade);
        }

        public void AdicionarItem(int produtoId, string nomeProduto, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            if (precoUnitario < 0)
                throw new ArgumentException("O preço não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(nomeProduto))
                throw new ArgumentException("O nome do produto não pode ser vazio.");

            var item = new ItemPedido(produtoId, nomeProduto, quantidade, precoUnitario);
            _itens.Add(item);
            CalcularValorTotal();
        }

        public void AlterarStatus(StatusPedido novoStatus)
        {
            // enviados não podem ser cancelados, e pedidos cancelados não podem ser alterados
            if (Status == StatusPedido.Enviado && novoStatus == StatusPedido.Cancelado)
                throw new InvalidOperationException("Não é possível cancelar um pedido já enviado.");

            Status = novoStatus;
            AtualizarData();
        }
        public void CancelarPedido()
        {
            if (Status == StatusPedido.Pago)
            {
                throw new InvalidOperationException("Não é permitido cancelar um pedido que já foi pago.");
            }

            Status = StatusPedido.Cancelado;
        }

    }
}
