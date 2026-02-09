import { Injectable, signal, computed, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/user.model'; 
import { map, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private platformId = inject(PLATFORM_ID);
  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:50932/api'; 

  private currentUserSignal = signal<Usuario | null>(this.getUserFromLocalStorage());

  readonly currentUser = computed(() => this.currentUserSignal());
  readonly isAuthenticated = computed(() => !!this.currentUserSignal());

  private getUserFromLocalStorage(): Usuario | null {
    if (isPlatformBrowser(this.platformId)) {
      const userJson = localStorage.getItem('currentUser');
      return userJson ? JSON.parse(userJson) : null;
    }
    return null;
  }

  getUsuarioLogado(): Usuario | null {
    return this.currentUser();
  }

  login(credentials: any): Observable<Usuario> {

    return this.http.post<Usuario>(`${this.apiUrl}/Auth/login`, credentials).pipe(
      map(user => {
        if (isPlatformBrowser(this.platformId)) {
          localStorage.setItem('currentUser', JSON.stringify(user));
          
          if ((user as any).token) {
            localStorage.setItem('token', (user as any).token);
          }
        }
        
        this.currentUserSignal.set(user);
        return user;
      })
    );
  }

  register(dados: any): Observable<any> {
    return this.http.post(`https://localhost:50932/api/usuario/registrar`, dados);
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('currentUser');
      localStorage.removeItem('token'); 
    }
    this.currentUserSignal.set(null);
  }

  hasRole(role: string): boolean {
    return this.currentUser()?.perfil === role;
  }
}