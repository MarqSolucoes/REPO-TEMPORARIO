import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard',
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard.component').then((m) => m.DashboardComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'obras',
    loadComponent: () => import('./pages/obras/obras.component').then((m) => m.ObrasComponent),
  },
  {
    path: 'obras/:id',
    loadComponent: () => import('./pages/obra-detalhe/obra-detalhe.component').then((m) => m.ObraDetalheComponent),
  },
  {
    path: 'compras/ordem-compra',
    loadComponent: () => import('./pages/compras/ordem-compra/ordem-compra.component').then((m) => m.OrdemCompraComponent),
  },
  {
    path: 'compras/cotacao',
    loadComponent: () => import('./pages/compras/cotacao/cotacao.component').then((m) => m.CotacaoComponent),
  },
  {
    path: 'compras/conciliacao',
    loadComponent: () => import('./pages/compras/conciliacao/conciliacao.component').then((m) => m.ConciliacaoComponent),
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
