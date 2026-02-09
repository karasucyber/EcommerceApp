import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PedidoService } from '../../core/services/pedido.service';
import { CarteiraService } from '../../core/services/carteira.service'; 
import { PedidoTimelineComponent } from '../../core/timeline/pedido-timeline.component';
@Component({
  selector: 'app-detalhe-pedido',
  standalone: true,
  imports: [CommonModule, PedidoTimelineComponent],
  templateUrl: './detalhe-pedido.component.html',
  styleUrl: './detalhe-pedido.component.scss'
})
export class DetalhePedidoComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private pedidoService = inject(PedidoService);
  private carteiraService = inject(CarteiraService);

  pedido = signal<any>(null);
  saldoCarteira = signal<number>(0);
  carregando = signal<boolean>(true);

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.carregarDados(id);
    }
  }

  carregarDados(pedidoId: number) {
    this.carregando.set(true);
    
    this.pedidoService.obterPorId(pedidoId).subscribe({
      next: (data: any) => {
        this.pedido.set(data);
        
        if (data.statusId === 1) {
          this.buscarSaldo(data.clienteId);
        }
        this.carregando.set(false);
      },
      error: (err: any) => {
        alert('Erro ao carregar pedido.');
        this.router.navigate(['/meus-pedidos']);
      }
    });
  }

  buscarSaldo(clienteId: number) {
    this.carteiraService.obterSaldoEExtrato(clienteId).subscribe((res: any) => {
      this.saldoCarteira.set(res.saldoAtual);
    });
  }

  pagarComCashback() {
    const pedidoId = this.pedido().id;
    
    this.carteiraService.pagarPedido(pedidoId).subscribe({
      next: (res: any) => {
        alert('Pagamento realizado com sucesso!');
        this.carregarDados(pedidoId);
      },
      error: (err: any) => alert('Erro no pagamento: ' + (err.error?.message || err.message))
    });
  }

  cancelarPedido() {
    if (!confirm('Tem certeza? O valor será estornado para sua carteira.')) return;

    const pedidoId = this.pedido().id;
    
    this.pedidoService.cancelar(pedidoId).subscribe({
      next: () => {
        alert('Pedido cancelado com sucesso.');
        this.carregarDados(pedidoId);
      },
      error: (err: any) => alert('Erro ao cancelar: ' + (err.error?.message || err.message))
    });
  }

  voltar() {
    this.router.navigate(['/meus-pedidos']);
  }
}