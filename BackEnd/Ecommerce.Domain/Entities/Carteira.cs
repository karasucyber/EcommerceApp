namespace Ecommerce.Domain.Entities
{
    public class Carteira : EntidadeBase
    {
        public int ClienteId { get; private set; }
        public decimal Saldo { get; private set; }

        protected Carteira() { }

        public Carteira(int clienteId)
        {
            ClienteId = clienteId;
            Saldo = 0;
        }

        public void AdicionarCashback(decimal valor)
        {
            if (valor <= 0) throw new ArgumentException("O valor deve ser positivo.");
            Saldo += valor;
            AtualizarData();
        }

        public void DebitarSaldo(decimal valor)
        {
            if (valor > Saldo) throw new InvalidOperationException("Saldo de cashback insuficiente.");
            Saldo -= valor;
            AtualizarData();
        }
    }
}