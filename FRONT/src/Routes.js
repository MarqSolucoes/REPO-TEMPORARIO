import Vue from 'vue';
import Router from 'vue-router';


import Layout from '@/components/Layout/Layout';

import Cidade from '@/pages/Configuracoes/Cidade/Cidade';
import Cargo from '@/pages/Configuracoes/Cargo/Cargo';
import Cliente from '@/pages/Configuracoes/Cliente/Cliente';
import Fornecedor from '@/pages/Configuracoes/Fornecedor/Fornecedor';
import Material from '@/pages/Configuracoes/Material/Material/Material';
import MaterialCategoria from '@/pages/Configuracoes/Material/MaterialCategoria/MaterialCategoria';
import Perfil from '@/pages/Configuracoes/Perfil/Perfil';
import Usuarios from '@/pages/Configuracoes/Usuario/Usuario';

import CustoFixo from '@/pages/Financeiro/CustoFixo/CustoFixo';
import DEF from '@/pages/Financeiro/DEF/DEF';
import ETO from '@/pages/Financeiro/ETO/ETO';
import Faturamento from '@/pages/Financeiro/Faturamento/Faturamento';
import NotasFiscais from '@/pages/Financeiro/NotasFiscais/NotasFiscais';
import FolhaPagamento from '@/pages/Financeiro/FolhaPagamento/FolhaPagamento';
import PedidoInterno from '@/pages/Financeiro/PedidoInterno/PedidoInterno';
import PedidoInternoRecorrente from '@/pages/Financeiro/PedidoInternoRecorrente/PedidoInternoRecorrente';
import ResumoFinanceiro from '@/pages/Financeiro/ResumoFinanceiro/ResumoFinanceiro';
import FluxoCaixaConsolidado from '@/pages/Financeiro/FluxoCaixaConsolidado/FluxoCaixaConsolidado';
import Entradas from '@/pages/Financeiro/Entradas/Entradas';

import OrdemCompra from '@/pages/Compras/OrdemCompra/OrdemCompra';
import Conciliacao from '@/pages/Compras/Conciliacao/Conciliacao';
import Cotacao from '@/pages/Compras/OrdemCompra/Cotacao';

import Dashboard from '@/pages/Dashboard/Dashboard';
import Login from '@/pages/Login/Login';

import Obras from '@/pages/Obras/Obras';
import ObraDetalhe from '@/pages/Obras/ObraDetalhe';

import RelatorioFaturamento from '@/pages/Relatorios/RelatorioFaturamento/RelatorioFaturamento';
import RelatorioAgenda from '@/pages/Relatorios/RelatorioAgenda/RelatorioAgenda';
import RelatorioAgendaFaturamento from '@/pages/Relatorios/RelatorioAgenda/RelatorioAgendaFaturamento';
import RelatorioETO from '@/pages/Relatorios/RelatorioETO/RelatorioETO';
import RelatorioPedidoCompra from '@/pages/Relatorios/RelatorioPedidoCompra/RelatorioPedidoCompra';
import RelatorioHistorico from '@/pages/Relatorios/RelatorioHistorico/RelatorioHistorico';
import RelatorioMaterial from '@/pages/Relatorios/RelatorioMaterial/RelatorioMaterial';

import RH from '@/pages/RH/RH/RH';

import ErrorPage from '@/pages/Error/Error';

import { isAuthenticated } from './mixins/auth';

Vue.use(Router);

export default new Router({
  routes: [
    {path: '/', redirect: '/app/dashboard'},
    {path: '/app', redirect: '/app/dashboard'},
    {
      path: '/login',
      name: 'Login',
      component: Login,
    },

    
    // {
    //   path: '/app/compras/cotacao/:id',
    //   name: 'Cotacao',
    //   component: Cotacao,
    // },

    {
      path: '/app',
      name: 'Layout',
      component: Layout,
      beforeEnter: (to, from, next) => {
        let token = localStorage.getItem('token');
        isAuthenticated(token) ? next() : next({path: '/login'});
      },
      children: [
        {
          path: 'configuracoes/cidade',
          name: 'Cidade',
          component: Cidade,
        },
        {
          path: 'configuracoes/cliente',
          name: 'Cliente',
          component: Cliente,
        },
        {
          path: 'configuracoes/cargo',
          name: 'Cargo',
          component: Cargo,
        },
        {
          path: 'configuracoes/fornecedor',
          name: 'Fornecedor',
          component: Fornecedor,
        },
        {
          path: 'configuracoes/material/material',
          name: 'Material',
          component: Material,
        },
        {
          path: 'configuracoes/material/materialCategoria',
          name: 'MaterialCategoria',
          component: MaterialCategoria,
        },
        {
          path: 'configuracoes/perfil',
          name: 'Perfil',
          component: Perfil,
        },
        {
          path: 'configuracoes/usuario',
          name: 'Usuarios',
          component: Usuarios,
        },
        {
          path: 'financeiro/custoFixo',
          name: 'CustoFixo',
          component: CustoFixo,
        },
        {
          path: 'financeiro/DEF',
          name: 'DEF',
          component: DEF,
        },
        {
          path: 'financeiro/ETO',
          name: 'ETO',
          component: ETO,
        },
        {
          path: 'financeiro/Entradas',
          name: 'Entradas',
          component: Entradas,
        },
        {
          path: 'financeiro/faturamento',
          name: 'Faturamento',
          component: Faturamento,
        },
        {
          path: 'financeiro/notasFiscais',
          name: 'NotasFiscais',
          component: NotasFiscais,
        },
        {
          path: 'financeiro/folhaPagamento',
          name: 'FolhaPagamento',
          component: FolhaPagamento,
        },
        {
          path: 'financeiro/pedidoInterno',
          name: 'PedidoInterno',
          component: PedidoInterno,
        },
        {
          path: 'financeiro/pedidoInternoRecorrente',
          name: 'PedidoInternoRecorrente',
          component: PedidoInternoRecorrente,
        },
        {
          path: 'financeiro/resumoFinanceiro',
          name: 'ResumoFinanceiro',
          component: ResumoFinanceiro,
        },
        {
          path: 'financeiro/fluxoCaixaConsolidado',
          name: 'FluxoCaixaConsolidado',
          component: FluxoCaixaConsolidado,
        },
        {
          path: 'relatorio/faturamento',
          name: 'RelatorioFaturamento',
          component: RelatorioFaturamento,
        },
        {
          path: 'relatorio/agenda',
          name: 'RelatorioAgenda',
          component: RelatorioAgenda,
        },
        {
          path: 'relatorio/agendaFaturamento',
          name: 'RelatorioAgendaFaturamento',
          component: RelatorioAgendaFaturamento,
        },
        {
          path: 'relatorio/ETO',
          name: 'RelatorioETO',
          component: RelatorioETO,
        },
        {
          path: 'relatorio/pedidoCompra',
          name: 'RelatorioPedidoCompra',
          component: RelatorioPedidoCompra,
        },
        {
          path: 'relatorio/historico',
          name: 'RelatorioHistorico',
          component: RelatorioHistorico,
        },
        {
          path: 'relatorio/material',
          name: 'RelatorioMaterial',
          component: RelatorioMaterial,
        },
        {
          path: 'compras/ordemCompra/:tag',
          name: 'OrdemCompra',
          component: OrdemCompra,
        },
        {
          path: 'compras/conciliacao',
          name: 'Conciliacao',
          component: Conciliacao,
        },
        {
          path: '/compras/cotacao/:id',
          name: 'Cotacao',
          component: Cotacao,
        },
        {
          path: 'dashboard',
          name: 'Dashboard',
          component: Dashboard,
        },
        {
          path: 'obras',
          name: 'Obras',
          component: Obras,
        },
        {
          path: 'obraDetalhe/:id',
          name: 'ObraDetalhe',
          component: ObraDetalhe,
        },
        {
          path: 'rh',
          name: 'RH',
          component: RH,
        },
      ],
    },
    {
      path: '*',
      name: 'Error',
      component: ErrorPage,
    }
  ],
});
