import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    const perfilExigido = route.data['perfil'];
    
    if (perfilExigido && authService.currentUser()?.perfil !== perfilExigido) {
      router.navigate(['/login']); 
      return false;
    }
    
    return true; 
  }

  router.navigate(['/login']); 
  return false;
};