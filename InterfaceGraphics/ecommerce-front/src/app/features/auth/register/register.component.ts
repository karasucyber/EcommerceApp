import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  form = { nome: '', email: '', senha: '', perfil: 1 }; 
  loading = false;

  cadastrar() {
    this.loading = true;

    this.authService.register(this.form).subscribe({
      next: () => {
        alert('Conta criada com sucesso! Faça login.');
        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        const mensagem = err.error?.message || 'Falha ao criar conta. Verifique os dados.';
        alert('Erro: ' + mensagem);
        this.loading = false;
        console.error(err);
      }
    });
  }
}