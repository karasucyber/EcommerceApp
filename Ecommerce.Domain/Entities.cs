namespace Ecommerce.Domain;

public enum PedidoStatus { Criado, Pago, Cancelado }

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string CPF { get; set; } = null!;
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
}

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public List<Produto> Produtos { get; set; } = new();
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public PedidoStatus Status { get; set; } = PedidoStatus.Criado;
    public decimal ValorTotal => Produtos.Sum(p => p.Preco);
}

public class PedidoHistorico
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string StatusAnterior { get; set; } = null!;
    public string StatusNovo { get; set; } = null!;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}