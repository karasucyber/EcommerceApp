import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarteiraService } from '../../core/services/carteira.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-minha-carteira',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './carteira.component.html'
})
export class MinhaCarteiraComponent implements OnInit {
  private carteiraService = inject(CarteiraService);
  private authService = inject(AuthService);

  saldo = signal<number>(0);

  ngOnInit() {
    this.carregarSaldo();
  }

  carregarSaldo() {
    const user = this.authService.getUsuarioLogado();
    const id = user ? user.id : 1; 

    this.carteiraService.obterSaldoEExtrato(id).subscribe({
      next: (res: any) => this.saldo.set(res.saldoAtual || 0),
      error: () => this.saldo.set(0) 
    });
  }

  depositar() {
    const user = this.authService.getUsuarioLogado();
    const id = user ? user.id : 1;
    
    const deposito = { clienteId: id, valor: 100.00, motivo: 'Depósito Teste' };
    
    this.carteiraService.creditar(deposito).subscribe(() => {
      alert('R$ 100,00 adicionados!');
      this.carregarSaldo();
    });
  }
}