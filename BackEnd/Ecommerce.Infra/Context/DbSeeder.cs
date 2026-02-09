//using Ecommerce.Domain.Entities;
//using Ecommerce.Infra.Context;
//using Ecommerce.Domain.Enum;

//namespace Ecommerce.Infra.Data
//{
//    public static class DbSeeder
//    {
//        public static void Seed(AppDbContext context)
//        {
//            Console.WriteLine(">>> Tentando iniciar o Seeder com grande volume de dados...");

//            // 1. CLIENTES
//            var clientes = new List<Cliente>
//            {
//                new Cliente("Marques Silva", "12345678901", "marques.cliente@gmail.com"),
//                new Cliente("GPS Logistica", "98765432100", "operacional@gps.com.br"),
//                new Cliente("Ana Tech", "11122233344", "ana.tech@provedor.com"),
//                new Cliente("Condominio Horizonte", "55566677788", "sindico@horizonte.com"),
//                new Cliente("Loja Central", "22233344455", "compras@lojacentral.com"),
//                new Cliente("Startup Alfa", "33344455566", "financeiro@alfa.com"),
//                new Cliente("Cliente Pessoa", "44455566677", "cliente.pessoa@gmail.com"),
//                new Cliente("Revenda Nordeste", "66677788899", "vendas@revenda.com")
//            };
//            context.Clientes.AddRange(clientes);
//            context.SaveChanges();

//            // 2. ENDEREÇO
//            var end1 = new Endereco("Rua de Teste", "123", "01000-000", "Sao Paulo");
//            end1.ClienteId = clientes[0].Id;
//            context.Enderecos.Add(end1);
//            context.SaveChanges();

//            // 3. PRODUTOS (p1 ao p12)
//            var produtos = new List<Produto>
//            {
//                new Produto("Servidor Dell", "SKU001", 12000.00m, 15500.00m, 50),
//                new Produto("Cabo HDMI", "SKU002", 50.00m, 85.00m, 500),
//                new Produto("Monitor UltraWide 34", "MON-34-003", 2500.00m, 3100.00m, 40),
//                new Produto("Cabo HDMI 2.0", "CAB-HDMI-004", 30.00m, 45.00m, 1000),
//                new Produto("Mouse Wireless Basic", "MOU-WL-005", 55.00m, 89.90m, 800),
//                new Produto("Nobreak 1500VA", "NOB-1500-006", 950.00m, 1450.00m, 30),
//                new Produto("SSD NVMe 1TB", "SSD-1TB-007", 450.00m, 650.00m, 200),
//                new Produto("Teclado Mecanico", "TEC-MEC-008", 120.00m, 250.00m, 150),
//                new Produto("Cabo de Rede CAT6", "CAB-CAT6-009", 8.00m, 15.00m, 2000),
//                new Produto("Impressora Laser", "IMP-LAS-010", 900.00m, 1400.00m, 20),
//                new Produto("Roteador Empresarial", "ROT-ENT-011", 800.00m, 1400.00m, 25),
//                new Produto("Headset Profissional", "HST-PRO-012", 150.00m, 320.00m, 100)
//            };
//            context.Produtos.AddRange(produtos);
//            context.SaveChanges();

//            // 4. GERAÇÃO AUTOMÁTICA DE 60 PEDIDOS
//            var random = new Random();
//            var listaPedidos = new List<Pedido>();

//            for (int i = 1; i <= 60; i++)
//            {
//                // Seleciona um cliente aleatório
//                var cliente = clientes[random.Next(clientes.Count)];
//                var pedido = new Pedido(cliente.Id, end1.Id);

//                // Adiciona de 1 a 3 produtos aleatórios em cada pedido
//                int itensNoPedido = random.Next(1, 4);
//                for (int j = 0; j < itensNoPedido; j++)
//                {
//                    var produto = produtos[random.Next(produtos.Count)];
//                    pedido.AdicionarItem(produto.Id, produto.Nome, random.Next(1, 5), produto.PrecoVenda);
//                }

//                // Distribui os status para testar os filtros do dashboard
//                int statusSorteado = random.Next(1, 6);
//                switch (statusSorteado)
//                {
//                    case 1: pedido.AlterarStatus(StatusPedido.AguardandoPagamento); break;
//                    case 2: pedido.AlterarStatus(StatusPedido.Pago); break;
//                    case 3:
//                        pedido.AlterarStatus(StatusPedido.Pago);
//                        pedido.AlterarStatus(StatusPedido.Enviado);
//                        break;
//                    case 4:
//                        pedido.AlterarStatus(StatusPedido.Pago);
//                        pedido.AlterarStatus(StatusPedido.Enviado);
//                        pedido.AlterarStatus(StatusPedido.Entregue);
//                        break;
//                    case 5: pedido.CancelarPedido(); break;
//                }

//                listaPedidos.Add(pedido);
//            }

//            context.Pedidos.AddRange(listaPedidos);
//            context.SaveChanges();

//            Console.WriteLine($">>> SUCESSO: Banco populado com {context.Produtos.Count()} produtos e {context.Pedidos.Count()} pedidos.");
//        }
//    }
//}