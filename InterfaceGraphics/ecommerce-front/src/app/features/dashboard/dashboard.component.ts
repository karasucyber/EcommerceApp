import { Component, OnInit, inject, PLATFORM_ID, signal } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RelatorioService, DashboardData } from '../../core/services/relatorio.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private relatorioService = inject(RelatorioService);
  private platformId = inject(PLATFORM_ID); 

  public data = signal<DashboardData | null>(null);
  public isBrowser = signal(false);

  ngOnInit() {
    this.isBrowser.set(isPlatformBrowser(this.platformId));
    if (this.isBrowser()) {
      this.carregarDados();
    }
  }

  carregarDados() {
    this.relatorioService.getDashboardFinanceiro().subscribe({
      next: (res) => this.data.set(res),
      error: (err) => console.error('Erro ao carregar dashboard', err)
    });
  }

  // Função auxiliar para definir a cor baseada na Classe ABC
  getClasseColor(classe: number): string {
    if (classe === 1) return '#2ecc71'; // Verde para Classe A
    if (classe === 2) return '#f1c40f'; // Amarelo para Classe B
    return '#e74c3c'; // Vermelho para Classe C
  }
}