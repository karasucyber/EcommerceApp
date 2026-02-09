import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  private http = inject(HttpClient);
  
  // URLs COMPLETAS para evitar erro 404
  private readonly API = 'https://localhost:50932/api/pedido';
  private readonly API_CARTEIRA = 'https://localhost:50932/api/carteira';

  obterPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.API}/${id}`);
  }

  // Backend espera PATCH para cancelar
  cancelar(id: number): Observable<any> {
    return this.http.patch(`${this.API}/${id}/cancelar`, {});
  }
  
pagar(id: number): Observable<any> {
    return this.http.post(
      `${this.API_CARTEIRA}/pagar-pedido/${id}`, 
      {}, 
      { responseType: 'text' } 
    );
  }

  listarMeusPedidos(clienteId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.API}/meus-pedidos/${clienteId}`);
  }
  
  criarPedido(dto: any): Observable<any> {
    return this.http.post<any>(this.API, dto);
  }
}