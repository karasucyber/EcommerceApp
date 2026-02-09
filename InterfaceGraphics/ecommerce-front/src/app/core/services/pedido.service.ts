import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  private http = inject(HttpClient);
  
  // CORREÇÃO: URL completa
  private readonly API = 'https://localhost:50932/api/pedido';

  listarMeusPedidos(clienteId: number): Observable<any> {
    return this.http.get<any>(`${this.API}/meus-pedidos/${clienteId}`);
  }

  obterPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.API}/${id}`);
  }

  criarPedido(pedido: any): Observable<any> {
    return this.http.post<any>(this.API, pedido);
  }

  cancelar(id: number): Observable<any> {
    return this.http.put(`${this.API}/${id}/cancelar`, {});
  }
}