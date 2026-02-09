export interface CriarPedidoItemDto {
  sku: string;
  quantidade: number;
}

export interface CriarPedidoDto {
  clienteId: number;
  enderecoId: number;
  itens: CriarPedidoItemDto[];
}