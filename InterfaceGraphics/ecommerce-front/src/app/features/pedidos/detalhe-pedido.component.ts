import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PedidoService } from '../../core/services/pedido.service';
import { CarteiraService } from '../../core/services/carteira.service';

@Component({
  selector: 'app-detalhe-pedido',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './detalhe-pedido.component.html',
  styleUrl: './detalhe-pedido.component.scss'
})
export class DetalhePedidoComponent implements OnInit { // <--- O ERRO ERA AQUI: TEM QUE TER 'export'
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private pedidoService = inject(PedidoService);
  private carteiraService = inject(CarteiraService);

  pedido = signal<any>(null);
  saldoCarteira = signal<number>(0);
  processando = signal(false);

  // --- ESTADO DO MODAL PADRÃO ---
  modal = {
    aberto: false,
    titulo: '',
    mensagem: '',
    tipo: 'info', 
    txtConfirmar: 'Confirmar',
    txtCancelar: 'Cancelar',
    acao: () => {} 
  };

  podeCancelar = computed(() => {
    const p = this.pedido();
    if (!p) return false;
    return !['Entregue', 'Cancelado', 'Enviado'].includes(p.status);
  });

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) this.carregarDados(id);
  }

  carregarDados(pedidoId: number) {
    this.pedidoService.obterPorId(pedidoId).subscribe({
      next: (data) => {
        this.pedido.set(data);
        if (data.status === 'Criado') this.buscarSaldo(data.clienteId);
      },
      error: () => this.router.navigate(['/vitrine'])
    });
  }

  buscarSaldo(clienteId: number) {
    this.carteiraService.obterSaldoEExtrato(clienteId).subscribe((res: any) => {
      this.saldoCarteira.set(res.saldoAtual ?? res.saldo ?? 0);
    });
  }

  // LÓGICA DE PAGAMENTO
  verificarPagamento() {
    const valor = this.pedido().valorTotal;
    const saldo = this.saldoCarteira();
    const falta = valor - saldo;

    if (saldo < valor) {
      this.configurarModal({
        titulo: 'Saldo Insuficiente',
        mensagem: `Você tem <strong>R$ ${saldo}</strong> e o pedido é <strong>R$ ${valor}</strong>.<br>Faltam R$ ${falta.toFixed(2)}.`,
        tipo: 'warning',
        txtConfirmar: 'Ir para Depósito',
        txtCancelar: 'Agora não',
        acao: () => {
          this.fecharModal();
          this.router.navigate(['/carteira']);
        }
      });
      return;
    }

    this.configurarModal({
      titulo: 'Confirmar Pagamento',
      mensagem: `Deseja pagar <strong>R$ ${valor}</strong> com seu saldo?`,
      tipo: 'success',
      txtConfirmar: 'Pagar Agora',
      acao: () => this.efetuarPagamentoReal()
    });
  }

  efetuarPagamentoReal() {
    this.processando.set(true);
    this.pedidoService.pagar(this.pedido().id).subscribe({
      next: () => {
        this.processando.set(false);
        this.fecharModal();
        alert('Pagamento realizado! ✅');
        this.carregarDados(this.pedido().id);
      },
      error: (err) => {
        this.processando.set(false);
        this.fecharModal();
        alert('Erro: ' + (err.error || 'Falha no pagamento.'));
      }
    });
  }

  // LÓGICA DE CANCELAMENTO
  prepararCancelamento() {
    const isPago = this.pedido().status === 'Pago';
    const msg = isPago 
      ? `O valor será <strong>estornado integralmente</strong>.` 
      : `O pedido será cancelado sem custos.`;

    this.configurarModal({
      titulo: 'Cancelar Pedido?',
      mensagem: `Tem certeza? <br><small>${msg}</small>`,
      tipo: 'danger',
      txtConfirmar: 'Sim, Cancelar',
      acao: () => this.efetuarCancelamentoReal()
    });
  }

  efetuarCancelamentoReal() {
    this.processando.set(true);
    this.pedidoService.cancelar(this.pedido().id).subscribe({
      next: () => {
        this.processando.set(false);
        this.fecharModal();
        this.carregarDados(this.pedido().id);
      },
      error: (err) => {
        this.processando.set(false);
        alert('Erro: ' + err.error?.message);
      }
    });
  }

  configurarModal(config: any) { this.modal = { ...this.modal, ...config, aberto: true }; }
  fecharModal() { this.modal.aberto = false; }
  executarAcaoModal() { if (this.modal.acao) this.modal.acao(); }
}