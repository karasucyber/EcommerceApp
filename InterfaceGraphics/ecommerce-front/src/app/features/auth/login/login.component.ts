import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  credentials = { email: '', senha: '' };
  errorMessage = '';

  onLogin() {
    this.authService.login(this.credentials).subscribe({
      next: (user) => {
        console.log('Autenticação bem-sucedida para o Grupo GPS');
        this.router.navigate(['/vitrine']);
      },
      error: (err) => {
        console.error('Falha no login:', err);
        this.errorMessage = 'Credenciais inválidas. Verifique seu e-mail corporativo e senha.';
      }
    });
  }
}