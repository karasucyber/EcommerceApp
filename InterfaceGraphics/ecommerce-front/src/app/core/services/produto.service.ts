import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Produto {
  id: number;
  nome: string;
  sku: string;
  precoCusto: number;
  precoVenda: number;
  estoqueAtual: number;
  imagemUrl?: string; 
}

export interface ResultadoVitrine {
  itens: Produto[]; 
  totalItens: number;
  pagina: number;
  totalPaginas: number;
}

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  private http = inject(HttpClient);
  

  private readonly API = 'https://localhost:50932/api/produto'; 

  obterVitrine(pagina: number = 1, tamanho: number = 10, termoBusca?: string): Observable<ResultadoVitrine> {
    let params = new HttpParams()
      .set('pagina', pagina)
      .set('tamanhoPagina', tamanho);

    if (termoBusca) {
      params = params.set('nome', termoBusca);
    }

    return this.http.get<ResultadoVitrine>(`${this.API}/vitrine`, { params });
  }

  ajustarEstoque(sku: string, quantidade: number): Observable<any> {
    return this.http.patch(`${this.API}/${sku}/estoque`, quantidade);
  }
}