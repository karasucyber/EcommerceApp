import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ClienteService {
  private http = inject(HttpClient);
  
  private readonly API = 'https://localhost:50932/api/cliente';

  cadastrar(dto: any): Observable<any> {
    return this.http.post(this.API, dto);
  }

  listar(): Observable<any[]> {
    return this.http.get<any[]>(this.API);
  }

  buscarPorCpf(cpf: string): Observable<any> {
    return this.http.get<any>(`${this.API}/buscar/${cpf}`);
  }
}