import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

import { LayoutComponent } from './shared/components/layout/layout.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ErrorComponent } from './pages/error/error.component';

import { CidadeComponent } from './pages/configuracoes/cidade/cidade.component';
import { CargoComponent } from './pages/configuracoes/cargo/cargo.component';
import { ClienteComponent } from './pages/configuracoes/cliente/cliente.component';
import { FornecedorComponent } from './pages/configuracoes/fornecedor/fornecedor.component';
import { MaterialComponent } from './pages/configuracoes/material/material/material.component';
import { MaterialCategoriaComponent } from './pages/configuracoes/material/material-categoria/material-categoria.component';
import { PerfilComponent } from './pages/configuracoes/perfil/perfil.component';
import { UsuarioComponent } from './pages/configuracoes/usuario/usuario.component';

import { CustoFixoComponent } from './pages/financeiro/custo-fixo/custo-fixo.component';
import { DefComponent } from './pages/financeiro/def/def.component';
import { EtoComponent } from './pages/financeiro/eto/eto.component';
import { EntradasComponent } from './pages/financeiro/entradas/entradas.component';
import { FaturamentoComponent } from './pages/financeiro/faturamento/faturamento.component';
import { FolhaPagamentoComponent } from './pages/financeiro/folha-pagamento/folha-pagamento.component';
import { FluxoCaixaConsolidadoComponent } from './pages/financeiro/fluxo-caixa-consolidado/fluxo-caixa-consolidado.component';
import { NotasFiscaisComponent } from './pages/financeiro/notas-fiscais/notas-fiscais.component';
import { PedidoInternoComponent } from './pages/financeiro/pedido-interno/pedido-interno.component';
import { PedidoInternoRecorrenteComponent } from './pages/financeiro/pedido-interno-recorrente/pedido-interno-recorrente.component';
import { ResumoFinanceiroComponent } from './pages/financeiro/resumo-financeiro/resumo-financeiro.component';

import { OrdemCompraComponent } from './pages/compras/ordem-compra/ordem-compra.component';
import { CotacaoComponent } from './pages/compras/cotacao/cotacao.component';
import { ConciliacaoComponent } from './pages/compras/conciliacao/conciliacao.component';

import { ObrasComponent } from './pages/obras/obras/obras.component';
import { ObraDetalheComponent } from './pages/obras/obra-detalhe/obra-detalhe.component';

import { RhComponent } from './pages/rh/rh.component';

import { RelatorioFaturamentoComponent } from './pages/relatorios/relatorio-faturamento/relatorio-faturamento.component';
import { RelatorioAgendaComponent } from './pages/relatorios/relatorio-agenda/relatorio-agenda.component';
import { RelatorioAgendaFaturamentoComponent } from './pages/relatorios/relatorio-agenda-faturamento/relatorio-agenda-faturamento.component';
import { RelatorioEtoComponent } from './pages/relatorios/relatorio-eto/relatorio-eto.component';
import { RelatorioPedidoCompraComponent } from './pages/relatorios/relatorio-pedido-compra/relatorio-pedido-compra.component';
import { RelatorioHistoricoComponent } from './pages/relatorios/relatorio-historico/relatorio-historico.component';
import { RelatorioMaterialComponent } from './pages/relatorios/relatorio-material/relatorio-material.component';

const routes: Routes = [
  { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'app',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'configuracoes/cidade', component: CidadeComponent },
      { path: 'configuracoes/cargo', component: CargoComponent },
      { path: 'configuracoes/cliente', component: ClienteComponent },
      { path: 'configuracoes/fornecedor', component: FornecedorComponent },
      { path: 'configuracoes/material/material', component: MaterialComponent },
      { path: 'configuracoes/material/materialCategoria', component: MaterialCategoriaComponent },
      { path: 'configuracoes/perfil', component: PerfilComponent },
      { path: 'configuracoes/usuario', component: UsuarioComponent },
      { path: 'financeiro/custoFixo', component: CustoFixoComponent },
      { path: 'financeiro/DEF', component: DefComponent },
      { path: 'financeiro/ETO', component: EtoComponent },
      { path: 'financeiro/Entradas', component: EntradasComponent },
      { path: 'financeiro/faturamento', component: FaturamentoComponent },
      { path: 'financeiro/notasFiscais', component: NotasFiscaisComponent },
      { path: 'financeiro/folhaPagamento', component: FolhaPagamentoComponent },
      { path: 'financeiro/pedidoInterno', component: PedidoInternoComponent },
      { path: 'financeiro/pedidoInternoRecorrente', component: PedidoInternoRecorrenteComponent },
      { path: 'financeiro/resumoFinanceiro', component: ResumoFinanceiroComponent },
      { path: 'financeiro/fluxoCaixaConsolidado', component: FluxoCaixaConsolidadoComponent },
      { path: 'relatorio/faturamento', component: RelatorioFaturamentoComponent },
      { path: 'relatorio/agenda', component: RelatorioAgendaComponent },
      { path: 'relatorio/agendaFaturamento', component: RelatorioAgendaFaturamentoComponent },
      { path: 'relatorio/ETO', component: RelatorioEtoComponent },
      { path: 'relatorio/pedidoCompra', component: RelatorioPedidoCompraComponent },
      { path: 'relatorio/historico', component: RelatorioHistoricoComponent },
      { path: 'relatorio/material', component: RelatorioMaterialComponent },
      { path: 'compras/ordemCompra/:tag', component: OrdemCompraComponent },
      { path: 'compras/conciliacao', component: ConciliacaoComponent },
      { path: 'compras/cotacao/:id', component: CotacaoComponent },
      { path: 'obras', component: ObrasComponent },
      { path: 'obraDetalhe/:id', component: ObraDetalheComponent },
      { path: 'rh', component: RhComponent },
    ]
  },
  { path: '**', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
