using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Enums;
using Xunit;

namespace Ecommerce.Tests.Domain
{
    public class PedidoTests
    {
        #region Testes de Status e Regras de Negócio

        [Fact]
        public void Deve_Criar_Pedido_Com_Status_Criado_E_Valor_Zero()
        {
            var pedido = new Pedido(1, 123);

            Assert.Equal(StatusPedido.Criado, pedido.Status);
            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact]
        public void Deve_Lancar_Erro_Ao_Cancelar_Pedido_Ja_Pago()
        {
            var pedido = new Pedido(1, 123);
            pedido.AlterarStatus(StatusPedido.Pago);

            var ex = Assert.Throws<InvalidOperationException>(() => pedido.CancelarPedido());
            Assert.Equal("Não é permitido cancelar um pedido que já foi pago.", ex.Message);
        }

        [Fact]
        public void Deve_Permitir_Cancelar_Pedido_Com_Status_Criado()
        {
            var pedido = new Pedido(1, 123);

            pedido.CancelarPedido();

            Assert.Equal(StatusPedido.Cancelado, pedido.Status);
        }

        [Fact]
        public void Deve_Lancar_Erro_Ao_Cancelar_Pedido_Ja_Enviado()
        {
            var pedido = new Pedido(1, 123);
            pedido.AlterarStatus(StatusPedido.Enviado);

            Assert.Throws<InvalidOperationException>(() => pedido.AlterarStatus(StatusPedido.Cancelado));
        }

        #endregion

        #region Testes de Cálculos e Itens

        [Theory]
        [InlineData(1, 100.00, 100.00)]
        [InlineData(2, 50.00, 100.00)]
        [InlineData(3, 33.33, 99.99)]
        [InlineData(10, 1.99, 19.90)]
        public void Deve_Calcular_Valor_Total_Baseado_Em_Quantidade_E_Preco(int qtd, decimal preco, decimal esperado)
        {
            var pedido = new Pedido(1, 123);

            pedido.AdicionarItem(101, "Produto Teste", qtd, preco);

            Assert.Equal(esperado, pedido.ValorTotal);
        }

        [Fact]
        public void Deve_Somar_Varios_Itens_No_Valor_Total()
        {
            var pedido = new Pedido(1, 123);

            pedido.AdicionarItem(1, "Notebook", 1, 3000.00m);
            pedido.AdicionarItem(2, "Mouse", 2, 50.00m);
            pedido.AdicionarItem(3, "Teclado", 1, 150.00m);

            Assert.Equal(3250.00m, pedido.ValorTotal);
        }

        [Fact]
        public void Deve_Manter_Integridade_Do_Snapshot_No_Item()
        {
            var pedido = new Pedido(1, 123);
            var nomeEsperado = "Cadeira Gamer Profissional";

            pedido.AdicionarItem(50, nomeEsperado, 1, 1200.00m);
            var item = pedido.Itens.First();

            Assert.Equal(nomeEsperado, item.NomeProdutoSnapshot);
        }

        #endregion

        #region Testes de Validação e Exceções

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Deve_Lancar_Erro_Ao_Adicionar_Item_Com_Quantidade_Invalida(int qtdInvalida)
        {
            var pedido = new Pedido(1, 123);

            Assert.Throws<ArgumentException>(() => pedido.AdicionarItem(1, "Produto", qtdInvalida, 10m));
        }

        [Fact]
        public void Deve_Lancar_Erro_Ao_Adicionar_Item_Com_Preco_Negativo()
        {
            var pedido = new Pedido(1, 123);

            Assert.Throws<ArgumentException>(() => pedido.AdicionarItem(1, "Produto", 1, -5.00m));
        }

        [Fact]
        public void Itens_Nao_Devem_Ser_Alterados_Externamente()
        {
            var pedido = new Pedido(1, 123);

            Assert.IsAssignableFrom<System.Collections.Generic.IReadOnlyCollection<ItemPedido>>(pedido.Itens);
        }

        #endregion
    }
}