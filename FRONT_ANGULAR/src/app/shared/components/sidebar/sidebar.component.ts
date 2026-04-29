import { Component, OnInit } from '@angular/core';
import { LayoutService } from '../../../core/services/layout.service';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {
  sidebarOpened = true;
  activeItem: string | null = null;

  cadastros = [
    { header: 'Cidades', link: '/app/configuracoes/cidade' },
    { header: 'Cargos', link: '/app/configuracoes/cargo' },
    { header: 'Clientes', link: '/app/configuracoes/cliente' },
    { header: 'DEF', link: '/app/financeiro/DEF' },
    { header: 'Fornecedores', link: '/app/configuracoes/fornecedor' },
    { header: 'Materiais / Serviços', link: '/app/configuracoes/material/material' },
    { header: 'Categorias Materiais', link: '/app/configuracoes/material/materialCategoria' },
    { header: 'Usuários', link: '/app/configuracoes/usuario' },
  ];

  financeiro = [
    { header: 'Agenda', link: '/app/relatorio/agenda' },
    { header: 'ETO', link: '/app/financeiro/ETO' },
    { header: 'Entradas', link: '/app/financeiro/Entradas' },
    { header: 'Faturamento', link: '/app/financeiro/faturamento' },
    { header: 'PI Recorrente', link: '/app/financeiro/pedidoInternoRecorrente' },
    { header: 'Fluxo Caixa', link: '/app/financeiro/fluxoCaixaConsolidado' },
  ];

  compras = [
    { header: 'Processo de Compra', link: '/app/compras/ordemCompra/EmCotacao' },
    { header: 'Conciliação NF', link: '/app/compras/conciliacao' },
  ];

  rh = [
    { header: 'RH', link: '/app/rh' },
  ];

  relatorios = [
    { header: 'Faturamento', link: '/app/relatorio/faturamento' },
    { header: 'Pedidos Compra', link: '/app/relatorio/pedidoCompra' },
    { header: 'Histórico', link: '/app/relatorio/historico' },
    { header: 'Materiais', link: '/app/relatorio/material' },
  ];

  constructor(private layout: LayoutService) {}

  ngOnInit(): void {
    this.layout.sidebarClose$.subscribe(v => this.sidebarOpened = !v);
    this.layout.sidebarActiveElement$.subscribe(v => this.activeItem = v);
  }
}
