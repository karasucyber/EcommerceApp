import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { VitrineComponent } from './features/vitrine/vitrine.component';
import { MeusPedidosComponent } from './features/pedidos/meus-pedidos.component';
import { DetalhePedidoComponent } from './features/pedidos/detalhe-pedido.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MinhaCarteiraComponent } from './features/carteira/carteira.component';
import { CadastroProdutoComponent } from './core/admin/CadastroProduto/cadastrar-produto.component';
import { CadastroClienteComponent } from './core/admin/CadastroCliente/cadastro-cliente.componente';
import { RegisterComponent } from './features/auth/register/register.component';
import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './core/main-layout/main-layout.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'vitrine', component: VitrineComponent },
      { path: 'carteira', component: MinhaCarteiraComponent },
      { path: 'meus-pedidos', component: MeusPedidosComponent },
      { path: 'pedido/:id', component: DetalhePedidoComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'cadastro-produto', component: CadastroProdutoComponent },
      { path: 'cadastro', component: CadastroClienteComponent },
    ]
  },

  { path: '**', redirectTo: 'login' }
];