import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map } from 'rxjs';

export interface ItemCurvaABC {
  produtoId: number;
  nome: string;
  quantidadeVendida: number;
  valorGerado: number;
  porcentagemNoFaturamento: number;
  classe: number; 
}

export interface DashboardData {
  faturamentoTotal: number;
  totalPedidos: number;
  ticketMedio: number;
  itensCurvaABC: ItemCurvaABC[]; 
}

@Injectable({ providedIn: 'root' })
export class RelatorioService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:50932/api/Relatorio'; 

  getDashboardFinanceiro(): Observable<DashboardData> {
    const financeiro$ = this.http.get<any>(`${this.apiUrl}/dashboard-financeiro`);
    const curvaAbc$ = this.http.get<ItemCurvaABC[]>(`${this.apiUrl}/curva-abc`);

    return forkJoin({ fin: financeiro$, abc: curvaAbc$ }).pipe(
      map(res => ({
        faturamentoTotal: res.fin.faturamentoTotal,
        totalPedidos: res.fin.totalPedidos,
        ticketMedio: res.fin.ticketMedio,
        itensCurvaABC: res.abc 
      }))
    );
  }
}