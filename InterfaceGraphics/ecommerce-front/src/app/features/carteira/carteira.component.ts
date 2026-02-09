import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CarteiraService } from '../../core/services/carteira.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-minha-carteira',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './carteira.component.html',
  styleUrl: './carteira.component.scss'
})
export class MinhaCarteiraComponent implements OnInit {
  private carteiraService = inject(CarteiraService);
  private authService = inject(AuthService);

  saldo = signal<number>(0);
  extrato = signal<any[]>([]);
  valorDeposito = signal<number>(0);
  loading = signal<boolean>(false);

  ngOnInit() {
    this.carregarDados();
  }

  isCredito(item: any): boolean {
    return item.tipo === 'Credito';
  }

  carregarDados() {
    this.loading.set(true);
    const user = this.authService.getUsuarioLogado();
    const clienteId = user ? user.id : 1; 

    this.carteiraService.obterSaldoEExtrato(clienteId).subscribe({
      next: (res: any) => {
        console.log('📦 JSON RECEBIDO:', res);
        
        this.saldo.set(res.saldoAtual || 0);


        let lista = res.historico || [];
        
        lista.sort((a: any, b: any) => {
           const dA = new Date(a.data).getTime();
           const dB = new Date(b.data).getTime();
           return dB - dA;
        });

        this.extrato.set(lista);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('❌ Erro:', err);
        this.loading.set(false);
      }
    });
  }

  depositar() {
    if (this.valorDeposito() <= 0) return alert('Valor inválido.');

    const user = this.authService.getUsuarioLogado();
    const dto = {
      clienteId: user ? user.id : 1,
      valor: this.valorDeposito(),
      motivo: 'Depósito via App'
    };

    this.loading.set(true);
    this.carteiraService.creditar(dto).subscribe({
      next: () => {
        alert('Depósito realizado! 💰');
        this.valorDeposito.set(0);
        this.carregarDados();
      },
      error: (err) => {
        alert('Erro: ' + (err.error?.message || 'Falha na conexão'));
        this.loading.set(false);
      }
    });
  }
}