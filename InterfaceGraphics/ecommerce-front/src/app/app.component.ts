import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common'; // Para usar o *ngIf
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component'; // Importe a Navbar
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    NavbarComponent 
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  public authService = inject(AuthService);
}