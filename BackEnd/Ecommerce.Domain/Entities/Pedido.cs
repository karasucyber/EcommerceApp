using Ecommerce.Domain.Enum;

namespace Ecommerce.Domain.Entities
{
    public class Pedido : EntidadeBase
    {
        public int ClienteId { get; private set; }
        public int EnderecoId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public StatusPedido Status { get; private set; }

        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens;

        protected Pedido() { }

        public Pedido(int clienteId, int enderecoId)
        {
            ClienteId = clienteId;
            EnderecoId = enderecoId;
            Status = StatusPedido.Criado; // Ponto de partida
            ValorTotal = 0;
        }

        public void AdicionarItem(int produtoId, string nomeProduto, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
            if (precoUnitario < 0) throw new ArgumentException("O preço não pode ser negativo.");

            var item = new ItemPedido(produtoId, nomeProduto, quantidade, precoUnitario);
            _itens.Add(item);

            CalcularValorTotal();
        }

        public void AlterarStatus(StatusPedido novoStatus)
        {
            if (Status == StatusPedido.Cancelado || Status == StatusPedido.Entregue)
                throw new InvalidOperationException($"Pedido já finalizado como {Status}. Não permite mais alterações.");

            switch (novoStatus)
            {
                case StatusPedido.Pago:
                    if (Status != StatusPedido.Criado && Status != StatusPedido.AguardandoPagamento)
                        throw new InvalidOperationException("Apenas pedidos Criados ou Aguardando Pagamento podem ser marcados como Pagos.");
                    break;

                case StatusPedido.Enviado:
                    if (Status != StatusPedido.Pago)
                        throw new InvalidOperationException("O pedido deve estar Pago antes de ser Enviado.");
                    break;

                case StatusPedido.Entregue:
                    if (Status != StatusPedido.Enviado)
                        throw new InvalidOperationException("O pedido deve ser Enviado antes de ser marcado como Entregue.");
                    break;
            }

            Status = novoStatus;
            AtualizarData(); 
        }

        public void CancelarPedido()
        {
            if (Status == StatusPedido.Enviado || Status == StatusPedido.Entregue)
                throw new InvalidOperationException("Não é possível cancelar um pedido que já saiu para entrega ou foi entregue.");

            Status = StatusPedido.Cancelado;
            AtualizarData();
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _itens.Sum(item => item.PrecoUnitario * item.Quantidade);
        }
    }
}