import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdutoService } from '../../core/services/produto.service';
import { CartService } from '../../core/services/cart.service';
import { CartModalComponent } from '../../shared/components/cart-modal/cart-modal.component'; // Importe o Modal

@Component({
  selector: 'app-vitrine',
  standalone: true,
  imports: [CommonModule, CartModalComponent], // Adicione o Modal aqui
  templateUrl: './vitrine.component.html',
  styleUrl: './vitrine.component.scss'
})
export class VitrineComponent implements OnInit {
  private produtoService = inject(ProdutoService);
  public cartService = inject(CartService); // Público para usar no HTML

  produtos = signal<any[]>([]);
  carregando = signal<boolean>(true);

  ngOnInit() {
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.produtoService.obterVitrine().subscribe({
      next: (res: any) => {
        const lista = res.itens ? res.itens : res;
        this.produtos.set(lista);
        this.carregando.set(false);
      },
      error: () => this.carregando.set(false)
    });
  }

  // Novo método: Apenas adiciona ao carrinho (não compra direto)
  adicionarAoCarrinho(produto: any) {
    this.cartService.adicionar(produto);
  }
}