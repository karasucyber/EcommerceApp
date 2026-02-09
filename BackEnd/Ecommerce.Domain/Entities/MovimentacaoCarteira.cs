using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities
{
    public class MovimentacaoCarteira : EntidadeBase
    {
        public int CarteiraId { get; private set; }
        public decimal Valor { get; private set; }
        public TipoMovimentacao Tipo { get; private set; } 
        public string Descricao { get; private set; }

        protected MovimentacaoCarteira() { }

        public MovimentacaoCarteira(int carteiraId, decimal valor, TipoMovimentacao tipo, string descricao)
        {
            CarteiraId = carteiraId;
            Valor = valor;
            Tipo = tipo;
            Descricao = descricao;
        }
    }
}