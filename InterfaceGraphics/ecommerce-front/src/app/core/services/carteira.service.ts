import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CarteiraService {
  private http = inject(HttpClient);
  
  // Confirme se a porta é a 50932 (vista no seu print de erro)
  private readonly API = 'https://localhost:50932/api/carteira';

  obterSaldoEExtrato(clienteId: number): Observable<any> {
    return this.http.get<any>(`${this.API}/meu-saldo/${clienteId}`);
  }

  creditar(dados: { clienteId: number, valor: number, motivo: string }): Observable<any> {
    return this.http.post(`${this.API}/creditar`, dados);
  }

  pagarPedido(pedidoId: number): Observable<any> {
    return this.http.post(`${this.API}/pagar-pedido/${pedidoId}`, {});
  }
}