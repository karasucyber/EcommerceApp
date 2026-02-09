import { Injectable, signal, computed } from '@angular/core';

export interface CartItem {
  produto: any;
  quantidade: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  itens = signal<CartItem[]>([]);
  aberto = signal<boolean>(false); 

  total = computed(() => this.itens().reduce((acc, item) => acc + (item.produto.precoVenda * item.quantidade), 0));
  qtdTotal = computed(() => this.itens().reduce((acc, item) => acc + item.quantidade, 0));

  adicionar(produto: any) {
    this.itens.update(lista => {
      const existente = lista.find(i => i.produto.sku === produto.sku);
      if (existente) {
        existente.quantidade++;
        return [...lista]; 
      }
      return [...lista, { produto, quantidade: 1 }];
    });
    this.abrir(); 
  }

  remover(sku: string) {
    this.itens.update(lista => lista.filter(i => i.produto.sku !== sku));
  }

  limpar() {
    this.itens.set([]);
  }

  abrir() { this.aberto.set(true); }
  fechar() { this.aberto.set(false); }
}