import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { PedidoService } from '../../core/services/pedido.service';

@Component({
  selector: 'app-meus-pedidos',
  standalone: true,
  imports: [CommonModule, RouterModule], 
  templateUrl: './meus-pedidos.component.html',
  styleUrl: './meus-pedidos.component.scss'
})
export class MeusPedidosComponent implements OnInit {
  private pedidoService = inject(PedidoService);
  private router = inject(Router);

  pedidos = signal<any[]>([]);
  carregando = signal<boolean>(true);

  ngOnInit() {
    this.carregarPedidos();
  }

  carregarPedidos() {
    const clienteId = 1; 

    this.pedidoService.listarMeusPedidos(clienteId).subscribe({
      next: (data) => {
        this.pedidos.set(data);
        this.carregando.set(false);
      },
      error: (err) => {
        console.error('Erro ao buscar pedidos', err);
        this.carregando.set(false);
      }
    });
  }

  verDetalhes(id: number) {
    this.router.navigate(['/pedido', id]);
  }
}