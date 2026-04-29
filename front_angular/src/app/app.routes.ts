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
    path: 'financeiro/entradas',
    loadComponent: () => import('./pages/financeiro/entradas/entradas.component').then((m) => m.EntradasComponent),
  },
  {
    path: 'financeiro/notas-fiscais',
    loadComponent: () => import('./pages/financeiro/notas-fiscais/notas-fiscais.component').then((m) => m.NotasFiscaisComponent),
  },
  {
    path: 'financeiro/resumo-financeiro',
    loadComponent: () => import('./pages/financeiro/resumo-financeiro/resumo-financeiro.component').then((m) => m.ResumoFinanceiroComponent),
  },
  {
    path: 'financeiro/faturamento',
    loadComponent: () => import('./pages/financeiro/faturamento/faturamento.component').then((m) => m.FaturamentoComponent),
  },
  {
    path: 'financeiro/folha-pagamento',
    loadComponent: () => import('./pages/financeiro/folha-pagamento/folha-pagamento.component').then((m) => m.FolhaPagamentoComponent),
  },
  {
    path: 'financeiro/custo-fixo',
    loadComponent: () => import('./pages/financeiro/custo-fixo/custo-fixo.component').then((m) => m.CustoFixoComponent),
  },
  { path: 'financeiro/def', loadComponent: () => import('./pages/financeiro/def/def.component').then((m) => m.DefComponent), },
  { path: 'financeiro/eto', loadComponent: () => import('./pages/financeiro/eto/eto.component').then((m) => m.EtoComponent), },
  { path: 'financeiro/fluxo-caixa-consolidado', loadComponent: () => import('./pages/financeiro/fluxo-caixa-consolidado/fluxo-caixa-consolidado.component').then((m) => m.FluxoCaixaConsolidadoComponent), },
  { path: 'financeiro/pedido-interno', loadComponent: () => import('./pages/financeiro/pedido-interno/pedido-interno.component').then((m) => m.PedidoInternoComponent), },
  { path: 'financeiro/pedido-interno-recorrente', loadComponent: () => import('./pages/financeiro/pedido-interno-recorrente/pedido-interno-recorrente.component').then((m) => m.PedidoInternoRecorrenteComponent), },
  { path: 'configuracoes', loadComponent: () => import('./pages/configuracoes/configuracoes.component').then((m) => m.ConfiguracoesComponent), },
  { path: 'rh', loadComponent: () => import('./pages/rh/rh.component').then((m) => m.RhComponent), },
  { path: 'error', loadComponent: () => import('./pages/error/error.component').then((m) => m.ErrorComponent), },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
