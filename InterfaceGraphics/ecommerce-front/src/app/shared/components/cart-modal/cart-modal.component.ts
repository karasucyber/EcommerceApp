import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { PedidoService } from '../../../core/services/pedido.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-cart-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart-modal.component.html',
  styleUrl: './cart-modal.component.scss'
})
export class CartModalComponent {
  cartService = inject(CartService);
  private pedidoService = inject(PedidoService);
  private authService = inject(AuthService);
  private router = inject(Router);

  finalizando = false;

  finalizarCompra() {
    if (this.cartService.itens().length === 0) return;

    this.finalizando = true;
    const usuario = this.authService.getUsuarioLogado();
    const clienteId = usuario ? usuario.id : 1;

    const dto = {
      clienteId: clienteId,
      enderecoId: 1, 
      itens: this.cartService.itens().map(i => ({
        sku: i.produto.sku,
        quantidade: i.quantidade
      }))
    };

    this.pedidoService.criarPedido(dto).subscribe({
      next: (res: any) => {
        this.cartService.limpar();
        this.cartService.fechar();
        this.finalizando = false;
        this.router.navigate(['/pedido', res.pedidoId || res.id]);
      },
      error: (err) => {
        alert('Erro ao finalizar: ' + (err.error?.message || 'Tente novamente.'));
        this.finalizando = false;
      }
    });
  }
}