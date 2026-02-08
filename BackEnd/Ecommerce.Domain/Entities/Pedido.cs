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

        public void AdicionarItem(int produtoId, int quantidade, decimal precoUnitario)
        {
            var item = new ItemPedido(produtoId, quantidade, precoUnitario);
            _itens.Add(item);
            ValorTotal += (quantidade * precoUnitario);
        }

        public void AlterarStatus(StatusPedido novoStatus)
        {
            // enviados não podem ser cancelados, e pedidos cancelados não podem ser alterados
            if (Status == StatusPedido.Enviado && novoStatus == StatusPedido.Cancelado)
                throw new InvalidOperationException("Não é possível cancelar um pedido já enviado.");

            Status = novoStatus;
            AtualizarData();
        }
    }
}
