import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdutoService } from '../../core/services/produto.service';
import { CartService } from '../../core/services/cart.service';
import { CartModalComponent } from '../../shared/components/cart-modal/cart-modal.component'; // Importe o Modal
import { MainLayoutComponent } from '../../core/main-layout/main-layout.component';
@Component({
  selector: 'app-vitrine',
  standalone: true,
  imports: [CommonModule, CartModalComponent, MainLayoutComponent],
  templateUrl: './vitrine.component.html',
  styleUrl: './vitrine.component.scss'
})
export class VitrineComponent implements OnInit {
  private produtoService = inject(ProdutoService);
  public cartService = inject(CartService); 

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

  adicionarAoCarrinho(produto: any) {
    this.cartService.adicionar(produto);
  }
}