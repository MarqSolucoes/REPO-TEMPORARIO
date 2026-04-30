import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

import { WidgetComponent } from './shared/components/widget/widget.component';
import { LoaderComponent } from './shared/components/loader/loader.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';
import { NavLinkComponent } from './shared/components/sidebar/nav-link/nav-link.component';

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

@NgModule({
  declarations: [
    AppComponent,
    WidgetComponent,
    LoaderComponent,
    LayoutComponent,
    HeaderComponent,
    SidebarComponent,
    NavLinkComponent,
    LoginComponent,
    DashboardComponent,
    ErrorComponent,
    CidadeComponent,
    CargoComponent,
    ClienteComponent,
    FornecedorComponent,
    MaterialComponent,
    MaterialCategoriaComponent,
    PerfilComponent,
    UsuarioComponent,
    CustoFixoComponent,
    DefComponent,
    EtoComponent,
    EntradasComponent,
    FaturamentoComponent,
    FolhaPagamentoComponent,
    FluxoCaixaConsolidadoComponent,
    NotasFiscaisComponent,
    PedidoInternoComponent,
    PedidoInternoRecorrenteComponent,
    ResumoFinanceiroComponent,
    OrdemCompraComponent,
    CotacaoComponent,
    ConciliacaoComponent,
    ObrasComponent,
    ObraDetalheComponent,
    RhComponent,
    RelatorioFaturamentoComponent,
    RelatorioAgendaComponent,
    RelatorioAgendaFaturamentoComponent,
    RelatorioEtoComponent,
    RelatorioPedidoCompraComponent,
    RelatorioHistoricoComponent,
    RelatorioMaterialComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
