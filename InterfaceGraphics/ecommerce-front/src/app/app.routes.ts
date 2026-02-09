import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component'; // Importe o Register
import { VitrineComponent } from './features/vitrine/vitrine.component';
import { MeusPedidosComponent } from './features/pedidos/meus-pedidos.component';
import { DetalhePedidoComponent } from './features/pedidos/detalhe-pedido.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MinhaCarteiraComponent } from './features/carteira/carteira.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'vitrine', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { 
    path: '', 
    canActivate: [authGuard],
    children: [
      { path: 'vitrine', component: VitrineComponent },
      { path: 'carteira', component: MinhaCarteiraComponent }, 
      { path: 'meus-pedidos', component: MeusPedidosComponent },
      { path: 'pedido/:id', component: DetalhePedidoComponent },
      { path: 'dashboard', component: DashboardComponent }
    ]
  }
];