import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

// Sincronizado com o seu Enum C#
export enum StatusPedido {
  Criado = 1,
  AguardandoPagamento = 2,
  Pago = 3,
  Enviado = 4,
  Entregue = 5,
  Cancelado = 6
}

@Component({
  selector: 'app-pedido-timeline',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pedido-timeline.component.html',
  styleUrl: './pedido-timeline.component.scss'
})
export class PedidoTimelineComponent {
  @Input() statusAtual: number = 1; 

  public etapas = [
    { label: 'Criado', valor: StatusPedido.Criado },
    { label: 'Pago', valor: StatusPedido.Pago },
    { label: 'Enviado', valor: StatusPedido.Enviado },
    { label: 'Entregue', valor: StatusPedido.Entregue }
  ];

  isConcluida(valorEtapa: number): boolean {
    if (this.statusAtual === StatusPedido.Cancelado) return false;
    return this.statusAtual >= valorEtapa;
  }

  isAtiva(valorEtapa: number): boolean {
    return this.statusAtual === valorEtapa;
  }
}