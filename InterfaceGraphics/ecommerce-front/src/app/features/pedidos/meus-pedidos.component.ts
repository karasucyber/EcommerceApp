import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PedidoService } from '../../core/services/pedido.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-meus-pedidos',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './meus-pedidos.component.html',
  styleUrl: './meus-pedidos.component.scss'
})
export class MeusPedidosComponent implements OnInit {
  private pedidoService = inject(PedidoService);
  private authService = inject(AuthService);

  pedidos = signal<any[]>([]);
  loading = signal(true);
  
  // SINAL DO FILTRO (Isso estava faltando)
  filtroAtual = signal<string>('Todos');

  // LÓGICA COMPUTADA (Isso estava faltando e gerou o erro NG9)
  pedidosFiltrados = computed(() => {
    const todos = this.pedidos();
    const filtro = this.filtroAtual();

    if (filtro === 'Todos') return todos;
    
    // Filtra comparando o status
    return todos.filter(p => p.status === filtro);
  });

  ngOnInit() {
    const user = this.authService.getUsuarioLogado();
    const clienteId = user ? user.id : 1; // Fallback para teste se não tiver user
    this.carregarPedidos(clienteId);
  }

  carregarPedidos(clienteId: number) {
    this.pedidoService.listarMeusPedidos(clienteId).subscribe({
      next: (data) => {
        this.pedidos.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.loading.set(false);
      }
    });
  }

  // Função chamada pelos botões do HTML
  filtrar(status: string) {
    this.filtroAtual.set(status);
  }
}